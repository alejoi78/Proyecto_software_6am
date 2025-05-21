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
        string sql = "SELECT * FROM prueba.serie";
        try
        {
            var db = dbConnection();
            IEnumerable<Serie> lista = await db.QueryAsync<Serie>(sql, new { });
            return lista.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
        }
        return result;
    }

    public async Task<Boolean> guardarSeries(Serie serie)
    {
        using (var db = dbConnection())
        {
            await db.OpenAsync();
            using (var transaction = await db.BeginTransactionAsync())
            {
                try
                {
                    string sql = @"INSERT INTO prueba.serie 
                             (titulo, director, anio, link, temporadas, DuracionPorCapitulo, genero, calificacion, imagen) 
                             VALUES (@Titulo, @Director, @Anio, @Link, @Temporadas, @DuracionPorCapitulo, @Genero, @Calificacion, @Imagen)";

                    var parameters = new
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
                    };

                    Console.WriteLine($"Ejecutando SQL: {sql} con parámetros: {JsonSerializer.Serialize(parameters)}");

                    int result = await db.ExecuteAsync(sql, parameters, transaction);

                    await transaction.CommitAsync();

                    Console.WriteLine($"Filas afectadas: {result}");

                    return result > 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error en guardarSeries: {ex.Message}");
                    return false;
                }
            }
        }
    }

    public async Task<bool> actualizarSeries(Serie serie)
    {
        int result = 0;
        string sql = @"UPDATE prueba.serie 
                     SET titulo = @Titulo, 
                         director = @Director, 
                         anio = @Anio, 
                         link = @Link,
                         temporadas = @Temporadas,
                         duracionPorCapitulo = @DuracionPorCapitulo,
                         genero = @Genero,
                         calificacion = @Calificacion,
                         imagen = @Imagen
                     WHERE idserie = @IdSerie";

        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                result = await db.ExecuteAsync(sql, new
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
                    IdSerie = serie.IdSerie
                });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
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