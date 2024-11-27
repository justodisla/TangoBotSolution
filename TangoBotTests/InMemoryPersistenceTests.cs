using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangoBotAPI.DI;
using TangoBotAPI.Persistence;
using TangoBotAPI.Persistence.Examples;
using Xunit;

namespace TangoBotAPI.Tests
{
    public class InMemoryPersistenceTests
    {
        private readonly IPersistence<User> _persistence;

        public InMemoryPersistenceTests()
        {
            //TangoBotServiceProviderExp.AddSingletonService<IPersistence<User>>(typeof(InMemoryPersistence<User>).FullName);

            _persistence = TangoBotServiceProviderExp.GetTransientService<IPersistence<User>>(typeof(InMemoryPersistence<User>).FullName) ?? throw new System.Exception("Service not found");
            //_persistence = new InMemoryPersistence<User>();
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
    }
}
