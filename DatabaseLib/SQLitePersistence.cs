using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TangoBotAPI.Persistence;

namespace DatabaseLib
{
    public class SQLitePersistence<T> : IPersistence<T> where T : IEntity
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

        // Explicit interface implementation for non-generic IPersistence methods
        async Task<IEntity> IPersistence.CreateAsync(IEntity entity)
        {
            return await CreateAsync((T)entity);
        }

        async Task<IEntity?> IPersistence.ReadAsync(Guid id)
        {
            return await ReadAsync(id);
        }

        async Task<IEnumerable<IEntity>> IPersistence.ReadAllAsync()
        {
            var entities = await ReadAllAsync();
            return entities.Cast<IEntity>();
        }

        async Task<IEntity> IPersistence.UpdateAsync(IEntity entity)
        {
            return await UpdateAsync((T)entity);
        }

        async Task<bool> IPersistence.DeleteAsync(Guid id)
        {
            return await DeleteAsync(id);
        }

        async Task<bool> IPersistence.DeleteAsync(IEntity entity)
        {
            return await DeleteAsync(entity.Id);
        }

        async Task<bool> IPersistence.RemoveTableAsync(string tableName)
        {
            return await RemoveTableAsync(tableName);
        }

        async Task<IEnumerable<string>> IPersistence.ListTablesAsync()
        {
            return await ListTablesAsync();
        }

        // Generic methods
        public async Task<T> CreateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            EnsureTableExists(tableName);

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
                    INSERT INTO {tableName} (Id, Data)
                    VALUES (@id, @data);
                ";
            command.Parameters.AddWithValue("@id", entity.Id.ToString());
            command.Parameters.AddWithValue("@data", SerializeEntity(entity));

            await command.ExecuteNonQueryAsync();

            entity.AfterSave();
            return (T)entity;
        }

        public async Task<T?> ReadAsync(Guid id)
        {
            var tableName = typeof(T).GetMethod("GetEntityName")?.Invoke(null, null)?.ToString();
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table name could not be determined from entity type.");
            }

            EnsureTableExists(tableName);

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    SELECT Data
                    FROM {tableName}
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", id.ToString());

            var result = await command.ExecuteScalarAsync();
            return result != null ? DeserializeEntity(result.ToString() ?? string.Empty) : default(T);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            var tableName = typeof(T).GetMethod("GetEntityName")?.Invoke(null, null)?.ToString();
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table name could not be determined from entity type.");
            }

            EnsureTableExists(tableName);

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    SELECT Data
                    FROM {tableName};
                ";

            var entities = new List<T>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                entities.Add(DeserializeEntity(reader.GetString(0)));
            }

            return entities;
        }

        public async Task<T> UpdateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            EnsureTableExists(tableName);

            entity.BeforeSave();

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    UPDATE {tableName}
                    SET Data = @data
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", entity.Id.ToString());
            command.Parameters.AddWithValue("@data", SerializeEntity(entity));

            await command.ExecuteNonQueryAsync();

            entity.AfterSave();
            return (T)entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tableName = typeof(T).GetMethod("GetEntityName")?.Invoke(null, null)?.ToString();
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table name could not be determined from entity type.");
            }

            EnsureTableExists(tableName);

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    DELETE FROM {tableName}
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

        public async Task<bool> RemoveTableAsync(string tableName)
        {
            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                    DROP TABLE IF EXISTS {tableName};
                ";

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<string>> ListTablesAsync()
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

        private string SerializeEntity(IEntity entity)
        {
            // Implement serialization logic (e.g., JSON serialization)
            return System.Text.Json.JsonSerializer.Serialize(entity);
        }

        private T DeserializeEntity(string data)
        {
            // Deserialize to the concrete type
            return (T)(System.Text.Json.JsonSerializer.Deserialize<T>(data) ?? throw new Exception("Could not deserialize entity"));
        }
    }
}
