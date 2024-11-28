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
        }

        public async Task<TangoBotAPI.Persistence.ICollection<T>> GetCollectionAsync<T>(string collectionName) where T : IEntity
        {
            EnsureTableExists(collectionName);
            return new SQLiteCollection<T>(_databaseManager, collectionName);
        }

        public async Task<IEnumerable<string>> ListCollectionsAsync()
        {
            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT name
                    FROM sqlite_master
                    WHERE type = 'table';
                ";

            var tables = new List<string>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tables.Add(reader.GetString(0));
            }

            return tables;
        }

        public Task<bool> CreateCollectionAsync<T>(string collectionName) where T : IEntity
        {
            EnsureTableExists(collectionName);
            return Task.FromResult(true);
        }

        public async Task<bool> RemoveCollectionAsync(string collectionName)
        {
            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    DROP TABLE IF EXISTS {collectionName};
                ";

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private void EnsureTableExists(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Table name is not set");
            }

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                CREATE TABLE IF NOT EXISTS {tableName} (
                    Id TEXT PRIMARY KEY,
                    Data TEXT NOT NULL
                );
            ";

            command.ExecuteNonQuery();
        }

        

        private class SQLiteCollection<T> : TangoBotAPI.Persistence.ICollection<T> where T : IEntity
        {
            private readonly DatabaseManager _databaseManager;
            private readonly string _tableName;

            public SQLiteCollection(DatabaseManager databaseManager, string tableName)
            {
                _databaseManager = databaseManager;
                _tableName = tableName;
            }

            public async Task<T> CreateAsync(T entity)
            {
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
                        INSERT INTO {_tableName} (Id, Data)
                        VALUES (@id, @data);
                    ";
                command.Parameters.AddWithValue("@id", entity.Id.ToString());
                command.Parameters.AddWithValue("@data", SerializeEntity(entity));

                await command.ExecuteNonQueryAsync();

                entity.AfterSave();
                return entity;
            }

            public async Task<T?> ReadAsync(Guid id)
            {
                using var connection = new SqliteConnection(_databaseManager.ConnectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = $@"
                        SELECT Data
                        FROM {_tableName}
                        WHERE Id = @id;
                    ";
                command.Parameters.AddWithValue("@id", id.ToString());

                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    return DeserializeEntity(result.ToString() ?? string.Empty);
                }

                return default;
            }

            public async Task<IEnumerable<T>> ReadAllAsync()
            {
                var entities = new List<T>();

                using var connection = new SqliteConnection(_databaseManager.ConnectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = $@"
                        SELECT Data
                        FROM {_tableName};
                    ";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    entities.Add(DeserializeEntity(reader.GetString(0)));
                }

                return entities;
            }

            public async Task<T> UpdateAsync(T entity)
            {
                entity.BeforeSave();

                using var connection = new SqliteConnection(_databaseManager.ConnectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = $@"
                        UPDATE {_tableName}
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
                using var connection = new SqliteConnection(_databaseManager.ConnectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = $@"
                        DELETE FROM {_tableName}
                        WHERE Id = @id;
                    ";
                command.Parameters.AddWithValue("@id", id.ToString());

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }

            public async Task<bool> DeleteAsync(T entity)
            {
                return await DeleteAsync(entity.Id);
            }

            private string SerializeEntity(T entity)
            {
                // Implement serialization logic (e.g., JSON serialization)
                return System.Text.Json.JsonSerializer.Serialize(entity);
            }

            private T DeserializeEntity(string data)
            {
                // Deserialize to the concrete type
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<T>(data) ?? throw new Exception("Could not deserialize entity");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
