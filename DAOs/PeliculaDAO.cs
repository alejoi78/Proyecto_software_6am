using System;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Utilidades;
namespace Proyecto_software_6am.DAOs;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;

public class PeliculaDAO : IPeliculaDAO
{
    private readonly MySQLConfiguration _connectionString;

    public PeliculaDAO(MySQLConfiguration connectionString)
    {
        _connectionString = connectionString;
    }
    protected MySqlConnection dbConnection()
    {
        return _connectionString.dbConnection(); // Usa el método de la configuración
    }

    public async Task<List<Entidades.Pelicula>> listarPeliculas()
    {
        List<Pelicula> result = new List<Pelicula>();
        string sql = "SELECT * FROM prueba.pelicula";
        try
        {
            var db = dbConnection();
            IEnumerable<Pelicula> lista = await db.QueryAsync<Pelicula>(sql, new { });
            return lista.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
        }   
        return result;
    }

    public async Task<Pelicula> obtenerPorId(int id)
    {
        Pelicula pelicula = null;
        string sql = "SELECT * FROM prueba.pelicula WHERE idpelicula = @IdPelicula";

        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();

                pelicula = await db.QueryFirstOrDefaultAsync<Pelicula>(sql, new { IdPelicula = id });

                if (pelicula == null)
                {
                    Console.WriteLine($"No se encontró la película con ID: {id}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener película por ID: {ex.Message}");
        }

        return pelicula;
    }

    public async Task<Boolean> guardarPeliculas(Pelicula pelicula)
    {
        int result = 0;
        string sql = @"INSERT INTO prueba.pelicula 
                      (titulo, director, anio, link, duracionHoras, genero, calificacion, imagen)  
                      VALUES (@Titulo, @Director, @Anio, @Link, @DuracionHoras, @Genero, @Calificacion, @Imagen)";
        try
        {
            var db = dbConnection();
            result = await db.ExecuteAsync(sql, new
            {
                pelicula.Titulo,
                pelicula.Director,
                pelicula.Anio,
                pelicula.Link,
                pelicula.DuracionHoras,
                pelicula.Genero,
                pelicula.Calificacion,
                pelicula.Imagen
            });
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
        }
        return result > 0;
    }

    public async Task<bool> actualizarPeliculas(Pelicula pelicula)
    {
        int result = 0;
        string sql = @"UPDATE prueba.pelicula 
               SET titulo = @Titulo, 
                   director = @Director, 
                   anio = @Anio, 
                   link = @Link, 
                   duracionHoras = @DuracionHoras,
                   genero = @Genero,
                   calificacion = @Calificacion,
                   imagen = @Imagen
               WHERE idpelicula = @IdPelicula";

        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                result = await db.ExecuteAsync(sql, new
                {
                    pelicula.Titulo,
                    pelicula.Director,
                    pelicula.Anio,
                    pelicula.Link,
                    pelicula.DuracionHoras,
                    pelicula.Genero,
                    pelicula.Calificacion,
                    pelicula.Imagen,
                    IdPelicula = pelicula.IdPelicula
                });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> eliminarPeliculas(int id)
    {
        int result = 0;
        string sql = "DELETE FROM prueba.pelicula WHERE idPelicula = @IdPelicula";
        try
        {
            using (var db = dbConnection())
            {
                await db.OpenAsync();
                result = await db.ExecuteAsync(sql, new { IdPelicula = id });
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