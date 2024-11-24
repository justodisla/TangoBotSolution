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

        public SQLitePersistence(string dbPath = "")
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _databaseFilePath = string.IsNullOrEmpty(dbPath) ? Path.Combine(_dataDirectory, "database.db") : dbPath;

            // Ensure the directory exists
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            _databaseManager = new DatabaseManager(_databaseFilePath);
            _databaseManager.InitializeDatabase();
        }

        public async Task<IEntity> CreateAsync(IEntity entity)
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
            command.CommandText = @"
                    INSERT INTO Entities (Id, Data)
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
            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT Data
                    FROM Entities
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", id.ToString());

            var result = await command.ExecuteScalarAsync();
            return result != null ? DeserializeEntity(result.ToString() ?? string.Empty) : null;
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync()
        {
            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT Data
                    FROM Entities;
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
            entity.BeforeSave();

            using var connection = new SqliteConnection(_databaseManager.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                    UPDATE Entities
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
            command.CommandText = @"
                    DELETE FROM Entities
                    WHERE Id = @id;
                ";
            command.Parameters.AddWithValue("@id", id.ToString());

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
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
