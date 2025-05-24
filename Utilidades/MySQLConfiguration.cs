using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace Proyecto_software_6am.Utilidades
{
    public class MySQLConfiguration
    {
        private readonly string _connectionString;

        public MySQLConfiguration(string connectionString)
        {
            _connectionString = connectionString;

            if (!ProbarConexion())
            {
                Console.WriteLine("⚠️ No se pudo conectar. Intentando crear la base de datos...");
                CrearBaseDeDatosYTablas();
            }
            else
            {
                Console.WriteLine("✅ Conexión exitosa a la base de datos.");
            }
        }

        public MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        private bool ProbarConexion()
        {
            try
            {
                using (var conexion = dbConnection())
                {
                    conexion.Open();
                    conexion.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void CrearBaseDeDatosYTablas()
        {
            try
            {
                var builder = new MySqlConnectionStringBuilder(_connectionString)
                {
                    Database = "" // Nos conectamos al servidor, no a una base específica
                };

                using (var conexion = new MySqlConnection(builder.ToString()))
                {
                    conexion.Open();

                    string script = File.ReadAllText("Utilidades/Dump20250503.sql");
                    var comandos = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var comando in comandos)
                    {
                        string sql = comando.Trim();
                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            using (var cmd = new MySqlCommand(sql, conexion))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    Console.WriteLine("✅ Base de datos y tablas creadas correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al crear la base de datos:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
