using Microsoft.Data.SqlClient;

namespace HelpdeskApp.Data
{
    public static class DbHelper
    {
        private static string? _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static string ConnectionString
        {
            get => _connectionString ?? throw new InvalidOperationException("Connection string not configured. Call SetConnectionString first.");
            set => _connectionString = value;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
