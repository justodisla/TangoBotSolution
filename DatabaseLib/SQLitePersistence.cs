using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TangoBotAPI.Persistence;

namespace DatabaseLib
{
    public class SQLitePersistence : IPersistence
    {
        private readonly DatabaseManager _databaseManager;
        private readonly string _dataDirectory;
        private readonly string _databaseFilePath;
        private string _tableName = "";

        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                EnsureTableExists();
            }
        }

        public SQLitePersistence()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _databaseFilePath = Path.Combine(_dataDirectory, "database.db");

            // Ensure the directory exists
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            _databaseManager = new DatabaseManager(_databaseFilePath);
            _databaseManager.InitializeDatabase();
            EnsureTableExists();
        }

        public async Task<IEntity> CreateAsync(IEntity entity)
        {
            if(string.IsNullOrEmpty(TableName))
            {
                throw new Exception("Table name is not set");
            }

            entity.BeforeSave();

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            // Check if the entity already exists
            var existingEntity = await ReadAsync(entity.Id);
            if (existingEntity != null)
            {
                throw new SqliteException("UNIQUE constraint failed: Entities.Id", 19);
            }

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    INSERT INTO {TableName} (Id, Data)
                    VALUES (@id, @data);
                ";
            command.Parameters.AddWithValue("@id", entity.Id.ToString());
            command.Parameters.AddWithValue("@data", SerializeEntity(entity));

            await command.ExecuteNonQueryAsync();

            entity.AfterSave();
            return entity;
        }

        public async Task<IEntity> ReadAsync(Guid id)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("Table name is not set");
            }

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    SELECT Data
                    FROM {TableName}
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", id.ToString());

            var result = await command.ExecuteScalarAsync();
            return result != null ? DeserializeEntity(result.ToString() ?? string.Empty) : null;
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync()
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("Table name is not set");
            }

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    SELECT Data
                    FROM {TableName};
                ";

            var entities = new List<IEntity>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                entities.Add(DeserializeEntity(reader.GetString(0)));
            }

            return entities;
        }

        public async Task<IEntity> UpdateAsync(IEntity entity)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("Table name is not set");
            }

            entity.BeforeSave();

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    UPDATE {TableName}
                    SET Data = @data
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", entity.Id.ToString());
            command.Parameters.AddWithValue("@data", SerializeEntity(entity));

            await command.ExecuteNonQueryAsync();

            entity.AfterSave();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("Table name is not set");
            }

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    DELETE FROM {TableName}
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", id.ToString());

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private void EnsureTableExists()
        {
            if (string.IsNullOrWhiteSpace(TableName))
            {
                return;
            }

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                CREATE TABLE IF NOT EXISTS {TableName} (
                    Id TEXT PRIMARY KEY,
                    Data TEXT NOT NULL
                );
            ";

            command.ExecuteNonQuery();
        }

        private string SerializeEntity(IEntity entity)
        {
            // Implement serialization logic (e.g., JSON serialization)
            return System.Text.Json.JsonSerializer.Serialize(entity);
        }

        private IEntity DeserializeEntity(string data)
        {
            // Deserialize to the concrete type
            return (IEntity)(System.Text.Json.JsonSerializer.Deserialize<Entity>(data) ?? throw new Exception("Could not deserialize entity"));
        }
    }
}
