using System.Data.SqlClient;

namespace HelpdeskApp.Data
{
    /// <summary>
    /// Central database helper class.
    /// Provides a single point to create SQL Server connections
    /// using the connection string from appsettings.json.
    /// </summary>
    public static class DbHelper
    {
        private static string? _connectionString;

        /// <summary>
        /// Initializes the connection string. Call this once at startup or use the property.
        /// </summary>
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets or sets the connection string used for all database operations.
        /// </summary>
        public static string ConnectionString
        {
            get => _connectionString ?? throw new InvalidOperationException("Connection string not configured. Call SetConnectionString first.");
            set => _connectionString = value;
        }

        /// <summary>
        /// Creates and returns a new SqlConnection using the configured connection string.
        /// Always use this inside a 'using' statement to ensure proper disposal.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
