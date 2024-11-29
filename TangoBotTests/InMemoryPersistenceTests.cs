using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TangoBot.API.Persistence.Examples;
using TangoBot.API.Persistence;

namespace TangoBotAPI.Tests
{
    public class InMemoryPersistenceTests
    {
        private readonly InMemoryPersistence _persistence;

        public InMemoryPersistenceTests()
        {
            _persistence = new InMemoryPersistence();
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldCreateCollection()
        {
            // Act
            var result = await _persistence.CreateCollectionAsync<User>("Users");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetCollectionAsync_ShouldReturnCollection()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");

            // Act
            var collection = await _persistence.GetCollectionAsync<User>("Users");

            // Assert
            Assert.NotNull(collection);
        }

        [Fact]
        public async Task ListCollectionsAsync_ShouldReturnAllCollections()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            await _persistence.CreateCollectionAsync<User>("Orders");

            // Act
            var collections = await _persistence.ListCollectionsAsync();

            // Assert
            Assert.NotNull(collections);
            Assert.Contains("Users", collections);
            Assert.Contains("Orders", collections);
        }

        [Fact]
        public async Task RemoveCollectionAsync_ShouldRemoveCollection()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");

            // Act
            var result = await _persistence.RemoveCollectionAsync("Users");
            var collections = await _persistence.ListCollectionsAsync();

            // Assert
            Assert.True(result);
            Assert.DoesNotContain("Users", collections);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntity()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            var collection = await _persistence.GetCollectionAsync<User>("Users");
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var createdEntity = await collection.CreateAsync(user);

            // Assert
            Assert.NotNull(createdEntity);
            Assert.Equal(user.Id, createdEntity.Id);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnEntity()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            var collection = await _persistence.GetCollectionAsync<User>("Users");
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await collection.CreateAsync(user);

            // Act
            var retrievedEntity = await collection.ReadAsync(user.Id);

            // Assert
            Assert.NotNull(retrievedEntity);
            Assert.Equal(user.Id, retrievedEntity.Id);
        }

        [Fact]
        public async Task ReadAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            var collection = await _persistence.GetCollectionAsync<User>("Users");
            var user1 = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            var user2 = new User { Id = Guid.NewGuid(), Name = "Jane Doe", Email = "jane.doe@example.com" };
            await collection.CreateAsync(user1);
            await collection.CreateAsync(user2);

            // Act
            var entities = await collection.ReadAllAsync();

            // Assert
            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            var collection = await _persistence.GetCollectionAsync<User>("Users");
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await collection.CreateAsync(user);
            user.Name = "John Smith";

            // Act
            var updatedEntity = await collection.UpdateAsync(user);

            // Assert
            Assert.NotNull(updatedEntity);
            Assert.Equal(user.Id, updatedEntity.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            await _persistence.CreateCollectionAsync<User>("Users");
            var collection = await _persistence.GetCollectionAsync<User>("Users");
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await collection.CreateAsync(user);

            // Act
            var result = await collection.DeleteAsync(user.Id);
            var deletedEntity = await collection.ReadAsync(user.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedEntity);
        }
    }

    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public void BeforeSave() { }
        public void AfterSave() { }
        public string GetEntityName() => "User";

        public bool Validate()
        {
            return true;
        }
    }
}
