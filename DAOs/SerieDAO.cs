using System;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Utilidades;
namespace Proyecto_software_6am.DAOs;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;

    public class SerieDAO : ISerieDAO
    {
        private readonly MySQLConfiguration _connectionString;

        public SerieDAO(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return _connectionString.dbConnection(); 
        }

        public async Task<List<Entidades.Serie>> listarSeries()
        {
            List<Serie> result = new List<Serie>();
            string sql = "SELECT idserie, Titulo, Director, Anio, Link, Temporadas, DuracionPorCapitulo FROM prueba.serie";
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
            int result = 0;
            string sql = "insert into prueba.serie (titulo, director, anio, link, temporadas, duracionPorCapitulo) VALUES (@Titulo, @Director, @Anio, @Link, @Temporadas, @DuracionPorCapitulo)";
            try
            {
                var db = dbConnection();
                result = await db.ExecuteAsync(sql, new
                {
                    serie.Titulo,
                    serie.Director,
                    serie.Anio,
                    serie.Link,
                    serie.Temporadas,
                    serie.DuracionPorCapitulo
                });
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }

            return result > 0;
        }

        public async Task<bool> actualizarSeries(Serie serie)
        {
            int result = 0;
            string sql = @"UPDATE prueba.serie 
               SET titulo = @Titulo, 
                   director = @Director, 
                   anio = @Anio, 
                   Link = @Link,
                   temporadas = @Temporadas,
                   duracionPorCapitulo = @DuracionPorCapitulo
               WHERE idserie = @IdSerie";

            try
            {
                using (var db = dbConnection())
                {
                    await db.OpenAsync();
                    result = await db.ExecuteAsync(sql, serie);
                }
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

