using Microsoft.Data.Sqlite;
using System;

namespace TangoBot.DatabaseLib
{
    public class DatabaseManager
    {
        public string ConnectionString { get; }

        public DatabaseManager(string dbPath)
        {
            ConnectionString = $"Data Source={dbPath}";
        }

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Entities (
                    Id TEXT PRIMARY KEY,
                    Data TEXT
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}
