using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TangoBotAPI.Persistence;
using DatabaseLib;
using Xunit;
using TangoBotAPI.DI;

namespace DatabaseLib.Tests
{
    public class SQLitePersistenceTests : IDisposable
    {
        private readonly SQLitePersistence _persistence;
        private readonly string _dataDirectory;
        private readonly string _databaseFilePath;

        public SQLitePersistenceTests()
        {
            // Specify the directory and database file name for testing
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
            _databaseFilePath = Path.Combine(_dataDirectory, "test_database.db");

            TangoBotServiceProvider.AddService<IPersistence>(provider => new SQLitePersistence(), typeof(SQLitePersistence).Name);
            _persistence = TangoBotServiceProvider.GetSingletonService<IPersistence>(typeof(SQLitePersistence).Name) as SQLitePersistence ?? throw new System.Exception("Service not found");

            try
            {
                Directory.Delete(_databaseFilePath, true);
            }
            catch (Exception)
            {

                //throw;
            }

            // Ensure the directory exists
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            // Create an instance of SQLitePersistence with the test database file path
            //_persistence = new SQLitePersistence();
        
            _persistence.TableName = typeof(User).Name;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntity()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var createdEntity = await _persistence.CreateAsync(user); 

            // Assert
            Assert.NotNull(createdEntity);
            Assert.Equal(user.Id, createdEntity.Id);

            // Cleanup
            await _persistence.DeleteAsync(user.Id);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnEntity()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await _persistence.CreateAsync(user);

            // Act
            var retrievedEntity = await _persistence.ReadAsync(user.Id);

            // Assert
            Assert.NotNull(retrievedEntity);
            Assert.Equal(user.Id, retrievedEntity.Id);

            // Cleanup
            await _persistence.DeleteAsync(user.Id);
        }

        [Fact]
        public async Task ReadAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var user1 = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            var user2 = new User { Id = Guid.NewGuid(), Name = "Jane Doe", Email = "jane.doe@example.com" };
            await _persistence.CreateAsync(user1);
            await _persistence.CreateAsync(user2);

            // Act
            var entities = await _persistence.ReadAllAsync();

            // Assert
            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());

            // Cleanup
            await _persistence.DeleteAsync(user1.Id);
            await _persistence.DeleteAsync(user2.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await _persistence.CreateAsync(user);
            user.Name = "John Smith";

            // Act
            var updatedEntity = await _persistence.UpdateAsync(user);

            // Assert
            Assert.NotNull(updatedEntity);
            Assert.Equal(user.Id, updatedEntity.Id);
            Assert.Equal("John Smith", ((User)updatedEntity).Name);

            // Cleanup
            await _persistence.DeleteAsync(user.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            await _persistence.CreateAsync(user);

            // Act
            var result = await _persistence.DeleteAsync(user.Id);
            var deletedEntity = await _persistence.ReadAsync(user.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedEntity);
        }

        public void Dispose()
        {

            //Delete all the records in the database
            var entities = _persistence.ReadAllAsync().Result;
            foreach (var entity in entities)
            {
                _persistence.DeleteAsync(entity.Id);
            }

            // Cleanup the test database file after all tests
            if (File.Exists(_databaseFilePath))
            {
                File.Delete(_databaseFilePath);
            }
        }
    }

    // Example implementation of User class for testing purposes
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public bool Validate()
        {
            // Implement validation logic
            return true;
        }

        public void BeforeSave()
        {
            // Implement actions before saving
        }

        public void AfterSave()
        {
            // Implement actions after saving
        }
    }
}
