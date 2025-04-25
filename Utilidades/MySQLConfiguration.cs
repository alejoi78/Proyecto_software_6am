namespace Proyecto_software_6am.Utilidades
{
    using MySql.Data.MySqlClient; // ¡Directiva necesaria!

    public class MySQLConfiguration
    {
        private readonly string _connectionString;

        public MySQLConfiguration(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection dbConnection() // Tipo correcto: MySqlConnection
        {
            return new MySqlConnection(_connectionString); // Usa el constructor adecuado
        }
    }
}
