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
        string sql = " SELECT idserie,Titulo, Director, Anio, Link, Temporadas, DuracionPorCapitulo FROM prueba.serie ";
        try
        {
            var db = dbConnection();
            IEnumerable<Serie> lista = await db.QueryAsync<Serie>(sql, new { });

            return lista.ToList(); ;
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
                             (titulo, director, anio, link, temporadas, DuracionPorCapitulo) 
                             VALUES (@Titulo, @Director, @Anio, @Link, @Temporadas, @DuracionPorCapitulo)";

                    var parameters = new
                    {
                        serie.Titulo,
                        serie.Director,
                        serie.Anio,
                        serie.Link,
                        serie.Temporadas,
                        serie.DuracionPorCapitulo
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
                         duracionPorCapitulo = @DuracionPorCapitulo
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

}



