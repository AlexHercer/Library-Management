using MySql.Data.MySqlClient;

namespace Project.DAL
{
    public class DatabaseHelper
    {
        private const string PSW = "111107nhat;";

        private readonly string _connectionString = "Server=localhost;" +
                                                    "Database=LibrarySystem;" +
                                                    "User ID=root;" +
                                                    "Password=" + PSW;

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void ExecuteQuery(string query, MySqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string query, MySqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteScalar();
        }

        public MySqlDataReader ExecuteReader(string query, MySqlParameter[]? parameters = null)
        {
            var conn = GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(query, conn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}