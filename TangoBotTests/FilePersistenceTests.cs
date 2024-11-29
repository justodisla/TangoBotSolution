using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TangoBot.API.Persistence;
using TangoBot.DependecyInjection;
using TangoBot.FSPersistence;
using Xunit;

namespace FSPersistence.Tests
{
    public class FilePersistenceTests
    {
        private readonly FilePersistence _filePersistence;

        public FilePersistenceTests()
        {
            _filePersistence = TangoBotServiceLocator.GetSingletonService<IPersistence>(typeof(FilePersistence).FullName) as FilePersistence;
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldCreateCollection()
        {
            // Arrange
            var collectionName = "TestCollection";

            // Act
            var result = await _filePersistence.CreateCollectionAsync<TestEntity>(collectionName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetCollectionAsync_ShouldReturnCollection()
        {
            // Arrange
            var collectionName = "TestCollection";
            await _filePersistence.CreateCollectionAsync<TestEntity>(collectionName);

            // Act
            var collection = await _filePersistence.GetCollectionAsync<TestEntity>(collectionName);

            // Assert
            Assert.NotNull(collection);
        }

        [Fact]
        public async Task ListCollectionsAsync_ShouldReturnCollections()
        {
            // Arrange
            var collectionName = "TestCollection";
            await _filePersistence.CreateCollectionAsync<TestEntity>(collectionName);

            // Act
            var collections = await _filePersistence.ListCollectionsAsync();

            // Assert
            Assert.Contains(collectionName, collections);
        }

        [Fact]
        public async Task RemoveCollectionAsync_ShouldRemoveCollection()
        {
            // Arrange
            var collectionName = "TestCollection";
            await _filePersistence.CreateCollectionAsync<TestEntity>(collectionName);

            // Act
            var result = await _filePersistence.RemoveCollectionAsync(collectionName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task FileCollection_CreateAsync_ShouldCreateEntity()
        {
            // Arrange
            var collectionName = "TestCollection";
            var collection = await _filePersistence.GetCollectionAsync<TestEntity>(collectionName);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };

            // Act
            var createdEntity = await collection.CreateAsync(entity);

            // Assert
            Assert.Equal(entity.Id, createdEntity.Id);
            Assert.Equal(entity.Name, createdEntity.Name);
        }

        [Fact]
        public async Task FileCollection_ReadAsync_ShouldReturnEntity()
        {
            // Arrange
            var collectionName = "TestCollection";
            var collection = await _filePersistence.GetCollectionAsync<TestEntity>(collectionName);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
            await collection.CreateAsync(entity);

            // Act
            var readEntity = await collection.ReadAsync(entity.Id);

            // Assert
            Assert.NotNull(readEntity);
            Assert.Equal(entity.Id, readEntity.Id);
            Assert.Equal(entity.Name, readEntity.Name);
        }

        [Fact]
        public async Task FileCollection_UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var collectionName = "TestCollection";
            var collection = await _filePersistence.GetCollectionAsync<TestEntity>(collectionName);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
            await collection.CreateAsync(entity);
            entity.Name = "Updated Entity";

            // Act
            var updatedEntity = await collection.UpdateAsync(entity);

            // Assert
            Assert.Equal(entity.Id, updatedEntity.Id);
            Assert.Equal("Updated Entity", updatedEntity.Name);
        }

        [Fact]
        public async Task FileCollection_DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            var collectionName = "TestCollection";
            var collection = await _filePersistence.GetCollectionAsync<TestEntity>(collectionName);
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
            await collection.CreateAsync(entity);

            // Act
            var result = await collection.DeleteAsync(entity.Id);

            // Assert
            Assert.True(result);
        }

        private class TestEntity : IEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public void AfterSave()
            {
                //throw new NotImplementedException();
            }

            public void BeforeSave()
            {
                //throw new NotImplementedException();
            }

            public bool Validate()
            {
                return true;
                
            }
        }
    }
}
