using Microsoft.Data.Sqlite;
using System;

namespace TangoBot.DatabaseLib
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PriceData (
                    Id INTEGER PRIMARY KEY,
                    Date TEXT,
                    Open REAL,
                    High REAL,
                    Low REAL,
                    Close REAL,
                    Volume INTEGER,
                    SMA REAL
                );
            ";
            command.ExecuteNonQuery();
        }

        public void InsertPriceData(DateTime date, decimal open, decimal high, decimal low, decimal close, long volume, decimal sma)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO PriceData (Date, Open, High, Low, Close, Volume, SMA)
                VALUES (@date, @open, @high, @low, @close, @volume, @sma);
            ";
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@open", open);
            command.Parameters.AddWithValue("@high", high);
            command.Parameters.AddWithValue("@low", low);
            command.Parameters.AddWithValue("@close", close);
            command.Parameters.AddWithValue("@volume", volume);
            command.Parameters.AddWithValue("@sma", sma);

            command.ExecuteNonQuery();
        }
    }
}
