using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangoBotApi.Infrastructure;
using TangoBotApi.Persistence;
using Xunit;

namespace TangoBotApi.Tests
{
    public class InMemoryPersistenceTests
    {
        private readonly IPersistence _persistence;

        public InMemoryPersistenceTests()
        {
            _persistence = ServiceLocator.GetSingletonService<IPersistence>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity = new TestEntity { Id = 1, Name = "Test Entity" };

            // Act
            await _persistence.AddAsync(collectionName, entity);
            var result = await _persistence.GetByIdAsync<TestEntity>(collectionName, entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity1 = new TestEntity { Id = 1, Name = "Test Entity 1" };
            var entity2 = new TestEntity { Id = 2, Name = "Test Entity 2" };

            await _persistence.AddAsync(collectionName, entity1);
            await _persistence.AddAsync(collectionName, entity2);

            // Act
            var result = await _persistence.GetAllAsync<TestEntity>(collectionName);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity = new TestEntity { Id = 1, Name = "Test Entity" };

            await _persistence.AddAsync(collectionName, entity);

            // Act
            entity.Name = "Updated Entity";
            await _persistence.UpdateAsync(collectionName, entity);
            var result = await _persistence.GetByIdAsync<TestEntity>(collectionName, entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Entity", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity = new TestEntity { Id = 1, Name = "Test Entity" };

            await _persistence.AddAsync(collectionName, entity);

            // Act
            await _persistence.DeleteAsync<TestEntity>(collectionName, entity.Id);
            var result = await _persistence.GetByIdAsync<TestEntity>(collectionName, entity.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllEntitiesAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity1 = new TestEntity { Id = 1, Name = "Test Entity 1" };
            var entity2 = new TestEntity { Id = 2, Name = "Test Entity 2" };

            await _persistence.AddAsync(collectionName, entity1);
            await _persistence.AddAsync(collectionName, entity2);

            // Act
            var result = await _persistence.GetAllEntitiesAsync<TestEntity>(collectionName);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddEntityAsync_ShouldAddEntity()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity = new TestEntity { Id = 1, Name = "Test Entity" };

            // Act
            await _persistence.AddEntityAsync(collectionName, entity);
            var result = await _persistence.GetByIdAsync<TestEntity>(collectionName, entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
        }

        [Fact]
        public async Task RemoveEntityAsync_ShouldRemoveEntity()
        {
            // Arrange
            var collectionName = "testCollection";
            var entity = new TestEntity { Id = 1, Name = "Test Entity" };

            await _persistence.AddAsync(collectionName, entity);

            // Act
            await _persistence.RemoveEntityAsync(collectionName, entity);
            var result = await _persistence.GetByIdAsync<TestEntity>(collectionName, entity.Id);

            // Assert
            Assert.Null(result);
        }

        private class TestEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
