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
            ProbarConexionOCrearBD(); // Probar o crear BD al instanciar
        }

        public MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        private void ProbarConexionOCrearBD()
        {
            try
            {
                using (var conexion = dbConnection())
                {
                    conexion.Open();
                    Console.WriteLine("✅ Conexión exitosa a la base de datos.");
                    conexion.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("⚠️ No se pudo conectar. Intentando crear la base de datos...");
                CrearBaseDeDatosYTablas();
            }
        }

        private void CrearBaseDeDatosYTablas()
        {
            try
            {
                // 1. Conexión sin especificar base de datos (conecta al servidor)
                var builder = new MySqlConnectionStringBuilder(_connectionString)
                {
                    Database = "" // Eliminar base de datos para conectarse al servidor
                };

                using (var conexion = new MySqlConnection(builder.ToString()))
                {
                    conexion.Open();

                    // 2. Leer y ejecutar el archivo SQL
                    string script = File.ReadAllText("Utilidades/Dump20250503.sql");
                    var comandos = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var comando in comandos)
                    {
                        using (var cmd = new MySqlCommand(comando, conexion))
                        {
                            cmd.ExecuteNonQuery();
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
