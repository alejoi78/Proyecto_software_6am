using System;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Proyecto_software_6am.DAOs.Interfaces;
using Proyecto_software_6am.Entidades;
using Proyecto_software_6am.Servicio.Interfaces;
using Proyecto_software_6am.Utilidades;

namespace Proyecto_software_6am.DAOs
{
    public class EpisodioDAO : IEpisodioDAO
    {
        private readonly MySQLConfiguration _connectionString;

        public EpisodioDAO(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return _connectionString.dbConnection();
        }

        public async Task<bool> guardarEpisodios(Episodio episodio)
        {
            const string sql = @"INSERT INTO prueba.episodio 
                               (nombre, temporada, link) 
                               VALUES (@Nombre, @Temporada, @Link)";

            try
            {
                using var db = dbConnection();
                await db.OpenAsync();

                var result = await db.ExecuteAsync(sql, new
                {
                    episodio.Nombre,
                    episodio.Temporada,
                    episodio.Link
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en guardarEpisodios: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> eliminarEpisodios(int id)
        {
            const string sql = "DELETE FROM prueba.episodio WHERE idepisodio = @Id";

            try
            {
                using var db = dbConnection();
                await db.OpenAsync();

                var result = await db.ExecuteAsync(sql, new { Id = id });
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en eliminarEpisodios: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> buscarPorid(int id)
        {
            const string sql = "SELECT COUNT(1) FROM prueba.episodio WHERE idepisodio = @Id";

            try
            {
                using var db = dbConnection();
                await db.OpenAsync();

                var count = await db.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en buscarPorid: {ex.Message}");
                return false;
            }
        }
    }
}