using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Infrastructure;
using TangoBotApi.Persistence;
using Xunit;

namespace TangoBotApi.Tests.Infrastructure
{
    public class SQLitePersistenceTests
    {
        private readonly IPersistence _persistence;
        private readonly string _connectionString = "DataSource=:memory:";

        public SQLitePersistenceTests()
        {
            _persistence = new SQLitePersistence(_connectionString);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE TestEntities (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT
                );
            ";
            command.ExecuteNonQuery();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            await AddTestEntityAsync(1, "Entity1");
            await AddTestEntityAsync(2, "Entity2");

            // Act
            var entities = await _persistence.GetAllAsync<TestEntity>("TestEntities");

            // Assert
            Assert.Equal(2, entities.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            // Arrange
            await AddTestEntityAsync(1, "Entity1");

            // Act
            var entity = await _persistence.GetByIdAsync<TestEntity>("TestEntities", 1);

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("Entity1", entity.Name);
        }

        [Fact]
        public async Task AddAsync_AddsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };

            // Act
            await _persistence.AddAsync("TestEntities", entity);

            // Assert
            var addedEntity = await _persistence.GetByIdAsync<TestEntity>("TestEntities", 1);
            Assert.NotNull(addedEntity);
            Assert.Equal(1, addedEntity.Id);
            Assert.Equal("Entity1", addedEntity.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntity()
        {
            // Arrange
            await AddTestEntityAsync(1, "Entity1");
            var entity = new TestEntity { Id = 1, Name = "UpdatedEntity" };

            // Act
            await _persistence.UpdateAsync("TestEntities", entity);

            // Assert
            var updatedEntity = await _persistence.GetByIdAsync<TestEntity>("TestEntities", 1);
            Assert.NotNull(updatedEntity);
            Assert.Equal(1, updatedEntity.Id);
            Assert.Equal("UpdatedEntity", updatedEntity.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntity()
        {
            // Arrange
            await AddTestEntityAsync(1, "Entity1");

            // Act
            await _persistence.DeleteAsync<TestEntity>("TestEntities", 1);

            // Assert
            var deletedEntity = await _persistence.GetByIdAsync<TestEntity>("TestEntities", 1);
            Assert.Null(deletedEntity);
        }

        private async Task AddTestEntityAsync(int id, string name)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestEntities (Id, Name) VALUES (@Id, @Name)";
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", name);

            await command.ExecuteNonQueryAsync();
        }

        private class TestEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
