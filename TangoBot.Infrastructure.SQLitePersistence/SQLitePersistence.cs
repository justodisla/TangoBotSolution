using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TangoBotApi.Persistence;

namespace TangoBotApi.Infrastructure
{
    public class SQLitePersistence : IPersistence
    {
        private readonly string _connectionString;

        public SQLitePersistence(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new()
        {
            var entities = new List<TEntity>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {collectionName}";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var entity = new TEntity();
                MapReaderToEntity(reader, entity);
                entities.Add(entity);
            }

            return entities;
        }

        public async Task<TEntity?> GetByIdAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity, new()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {collectionName} WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var entity = new TEntity();
                MapReaderToEntity(reader, entity);
                return entity;
            }

            return null;
        }

        public async Task AddAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {collectionName} (Id, Name) VALUES (@Id, @Name)";
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {collectionName} SET Name = @Name WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {collectionName} WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new()
        {
            return GetAllAsync<TEntity>(collectionName);
        }

        public Task AddEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            return AddAsync(collectionName, entity);
        }

        public Task RemoveEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            return DeleteAsync<TEntity>(collectionName, entity.Id);
        }

        public string[] Requires()
        {
            return new string[0];
        }

        private void MapReaderToEntity<TEntity>(IDataReader reader, TEntity entity) where TEntity : class, IEntity
        {
            entity.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            entity.Name = reader.GetString(reader.GetOrdinal("Name"));
        }
    }
}
