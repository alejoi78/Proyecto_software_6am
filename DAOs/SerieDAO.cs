using System;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Utilidades;
namespace Proyecto_software_6am.DAOs;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class SerieDAO : ISerieDAO
{
    private readonly MySQLConfiguration _connectionString;

    public SerieDAO(MySQLConfiguration connectionString)
    {
        _connectionString = connectionString;
    }
    protected MySqlConnection dbConnection()
    {
        return _connectionString.dbConnection(); // Usa el método de la configuración
    }

    public async Task<List<Entidades.Serie>> listarSeries()
    {
        List<Serie> result = new List<Serie>();
        string sql = @"
SELECT 
    s.idserie, s.titulo, s.director, s.anio, s.link, s.temporadas, 
    s.DuracionPorCapitulo, s.genero, s.calificacion, s.imagen,
    e.idepisodio, e.Nombre, e.Temporada, e.Link, e.DuracionMin, e.idSerie
FROM prueba.serie s
LEFT JOIN prueba.episodio e ON s.idserie = e.idSerie
ORDER BY s.idserie, e.Temporada, e.idepisodio";

        try
        {
            var db = dbConnection();

            // Usamos QueryMultiple para manejar relaciones uno-a-muchos
            var seriesDict = new Dictionary<int, Serie>();

            await db.QueryAsync<Serie, Episodio, Serie>(sql, (serie, episodio) =>
            {
                if (!seriesDict.TryGetValue(serie.IdSerie, out var serieEntry))
                {
                    serieEntry = serie;
                    serieEntry.Episodios = new List<Episodio>();
                    seriesDict.Add(serieEntry.IdSerie, serieEntry);
                }

                if (episodio != null)
                {
                    serieEntry.Episodios.Add(episodio);
                }

                return serieEntry;
            }, splitOn: "idepisodio");

            return seriesDict.Values.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
        }
        return result;
    }

    public async Task<Serie> obtenerPorId(int id)
    {
        Serie serie = null;
        string sql = @"
    SELECT 
        s.idserie, s.titulo, s.director, s.anio, s.link, s.temporadas, 
        s.DuracionPorCapitulo, s.genero, s.calificacion, s.imagen,
        e.idepisodio, e.Nombre, e.Temporada, e.Link, e.DuracionMin, e.idSerie
    FROM prueba.serie s
    LEFT JOIN prueba.episodio e ON s.idserie = e.idSerie
    WHERE s.idserie = @IdSerie";

        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();

                var seriesDict = new Dictionary<int, Serie>();

                await db.QueryAsync<Serie, Episodio, Serie>(sql, (s, e) =>
                {
                    if (!seriesDict.TryGetValue(s.IdSerie, out var serieEntry))
                    {
                        serieEntry = s;
                        serieEntry.Episodios = new List<Episodio>();
                        seriesDict.Add(serieEntry.IdSerie, serieEntry);
                    }

                    if (e != null)
                    {
                        serieEntry.Episodios.Add(e);
                    }

                    return serieEntry;
                }, new { IdSerie = id }, splitOn: "idepisodio");

                serie = seriesDict.Values.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener serie por ID: {ex.Message}");
        }

        return serie;
    }


    public async Task<bool> guardarSeries(Serie serie)
    {
        Console.WriteLine($"Recibiendo serie con {serie.Episodios?.Count ?? 0} episodios");

        using (var db = dbConnection())
        {
            await db.OpenAsync();
            using (var transaction = await db.BeginTransactionAsync())
            {
                try
                {
                    // 1. Insertar la serie
                    string sqlSerie = @"INSERT INTO prueba.serie 
                    (titulo, director, anio, link, temporadas, DuracionPorCapitulo, genero, calificacion, imagen) 
                    VALUES (@Titulo, @Director, @Anio, @Link, @Temporadas, @DuracionPorCapitulo, @Genero, @Calificacion, @Imagen);
                    SELECT LAST_INSERT_ID();";

                    Console.WriteLine("Insertando serie...");
                    var idSerie = await db.ExecuteScalarAsync<int>(sqlSerie, new
                    {
                        serie.Titulo,
                        serie.Director,
                        serie.Anio,
                        serie.Link,
                        serie.Temporadas,
                        serie.DuracionPorCapitulo,
                        serie.Genero,
                        serie.Calificacion,
                        serie.Imagen
                    }, transaction);

                    Console.WriteLine($"Serie insertada con ID: {idSerie}");

                    // 2. Insertar episodios si existen
                    if (serie.Episodios != null && serie.Episodios.Any())
                    {
                        Console.WriteLine($"Insertando {serie.Episodios.Count} episodios...");
                        string sqlEpisodio = @"INSERT INTO prueba.episodio 
                        (Nombre, Temporada, Link, idSerie, DuracionMin) 
                        VALUES (@Nombre, @Temporada, @Link, @IdSerie, @DuracionMin)";

                        foreach (var episodio in serie.Episodios)
                        {
                            Console.WriteLine($"Insertando episodio: {episodio.Nombre}");
                            var result = await db.ExecuteAsync(sqlEpisodio, new
                            {
                                episodio.Nombre,
                                episodio.Temporada,
                                episodio.Link,
                                IdSerie = idSerie,
                                episodio.DuracionMin
                            }, transaction);
                            Console.WriteLine($"Episodio insertado: {result} filas afectadas");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No hay episodios para insertar");
                    }

                    await transaction.CommitAsync();
                    Console.WriteLine("Transacción completada con éxito");
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error en transacción: {ex.ToString()}");
                    return false;
                }
            }
        }
    }

    public async Task<bool> actualizarSeries(Serie serie)
    {
        using (var db = dbConnection())
        {
            await db.OpenAsync();
            using (var transaction = await db.BeginTransactionAsync())
            {
                try
                {
                    Console.WriteLine($"Actualizando serie ID: {serie.IdSerie} con {serie.Episodios?.Count ?? 0} episodios");

                    // 1. Actualizar la serie principal
                    string sqlSerie = @"UPDATE prueba.serie 
                    SET titulo = @Titulo, 
                        director = @Director, 
                        anio = @Anio, 
                        link = @Link,
                        temporadas = @Temporadas,
                        DuracionPorCapitulo = @DuracionPorCapitulo,
                        genero = @Genero,
                        calificacion = @Calificacion,
                        imagen = @Imagen
                    WHERE idserie = @IdSerie";

                    var serieResult = await db.ExecuteAsync(sqlSerie, new
                    {
                        serie.Titulo,
                        serie.Director,
                        serie.Anio,
                        serie.Link,
                        serie.Temporadas,
                        serie.DuracionPorCapitulo,
                        serie.Genero,
                        serie.Calificacion,
                        serie.Imagen,
                        serie.IdSerie
                    }, transaction);

                    Console.WriteLine($"Serie actualizada: {serieResult} filas afectadas");

                    // 2. Manejar episodios (eliminar existentes y agregar nuevos)
                    if (serie.Episodios != null)
                    {
                        // Primero eliminar episodios existentes
                        string sqlDeleteEpisodios = "DELETE FROM prueba.episodio WHERE idSerie = @IdSerie";
                        var deleteResult = await db.ExecuteAsync(sqlDeleteEpisodios, new { IdSerie = serie.IdSerie }, transaction);
                        Console.WriteLine($"Episodios eliminados: {deleteResult}");

                        // Luego insertar los nuevos episodios
                        if (serie.Episodios.Any())
                        {
                            Console.WriteLine($"Insertando {serie.Episodios.Count} nuevos episodios...");
                            string sqlEpisodio = @"INSERT INTO prueba.episodio 
                            (Nombre, Temporada, Link, idSerie, DuracionMin) 
                            VALUES (@Nombre, @Temporada, @Link, @IdSerie, @DuracionMin)";

                            foreach (var episodio in serie.Episodios)
                            {
                                Console.WriteLine($"Insertando episodio: {episodio.Nombre}");
                                var result = await db.ExecuteAsync(sqlEpisodio, new
                                {
                                    episodio.Nombre,
                                    episodio.Temporada,
                                    episodio.Link,
                                    IdSerie = serie.IdSerie,
                                    episodio.DuracionMin
                                }, transaction);
                                Console.WriteLine($"Episodio insertado: {result} filas afectadas");
                            }
                        }
                    }

                    await transaction.CommitAsync();
                    Console.WriteLine("Actualización completada con éxito");
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error en actualización: {ex.ToString()}");
                    return false;
                }
            }
        }
    }

    public async Task<bool> eliminarSeries(int id)
    {
        int result = 0;
        string sql = "DELETE FROM prueba.serie WHERE idserie = @Idserie";
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                result = await db.ExecuteAsync(sql, new { idserie = id });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar: " + ex.Message);
            return false;
        }
    }
}