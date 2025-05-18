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
        string sql = " SELECT idPelicula,Titulo, Director, Anio, Link, DuracionHoras, Genero, Calificacion  FROM prueba.pelicula ";
        try
        {
            var db = dbConnection();
            IEnumerable<Pelicula> lista = await db.QueryAsync<Pelicula>(sql, new { });
            
            return lista.ToList(); ;
        }
        catch (Exception ex)
        {
     
            Console.WriteLine("Error " + ex.Message);
        }
        return result;
    }

    public async Task<Boolean> guardarPeliculas(Pelicula pelicula)
    {
        int result = 0;
        string sql = "insert into prueba.pelicula (titulo, director, anio, link, duracionHoras, genero, calificacion)  values (@Titulo, @Director, @Anio, @Link, @DuracionHoras, @Genero, @Calificacion)";
        try
        {
            var db = dbConnection();
            result = await db.ExecuteAsync(sql, new { pelicula.Titulo, pelicula.Director, pelicula.Anio,pelicula.Link,pelicula.DuracionHoras, pelicula.Genero, pelicula.Calificacion });
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
                   calificacion = @Calificacion
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
                    pelicula.IdPelicula 
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


}


