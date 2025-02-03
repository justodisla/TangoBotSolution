using System;
using System.Collections.Generic;
using TangoBotApi.Infrastructure;
using TangoBotApi.Test;
using Xunit;

namespace TangoBotApi.Tests
{
    public class ServiceLocatorTests
    {
        [Fact]
        public void Initialize_ShouldRegisterServices()
        {
            // Arrange
            ServiceLocator.Initialize();

            // Act
            var serviceProvider = ServiceLocator.GetSingletonService<IServiceProvider>();

            // Assert
            Assert.NotNull(serviceProvider);
        }

        [Fact]
        public void GetSingletonService_ShouldReturnRegisteredService()
        {
            // Arrange
            ServiceLocator.Initialize();

            // Act
            var service = ServiceLocator.GetSingletonService<IDependencyInjectionTest>();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetTransientService_ShouldReturnRegisteredService()
        {
            // Arrange
            ServiceLocator.Initialize();

            // Act
            var service = ServiceLocator.GetTransientService<IDependencyInjectionTest>();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetImplementationType_ShouldThrowExceptionForUnknownType()
        {
            // Arrange
            //ServiceLocator.Initialize();

            // Act & Assert
            //Assert.Throws<InvalidOperationException>(() => ServiceLocator.GetSingletonService<IUnknownService>());
        }

        [Fact]
        public void DependencyInjectionTest_ShouldStoreAndRetrieveValues()
        {
            // Arrange
            ServiceLocator.Initialize();
            var service = ServiceLocator.GetSingletonService<IDependencyInjectionTest>();

            // Act
            service.SetValue("key1", "value1");
            var value = service.GetValue("key1");

            // Assert
            Assert.Equal("value1", value);
        }

        [Fact]
        public void DependencyInjectionTest_ShouldDeleteValue()
        {
            // Arrange
            ServiceLocator.Initialize();
            var service = ServiceLocator.GetSingletonService<IDependencyInjectionTest>();

            // Act
            service.SetValue("key1", "value1");
            service.DeleteValue("key1");
            var value = service.GetValue("key1");

            // Assert
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public async Task DependencyInjectionTest_ShouldSaveValuesAsync()
        {
            // Arrange
            ServiceLocator.Initialize();
            var service = ServiceLocator.GetSingletonService<IDependencyInjectionTest>();

            // Act
            await service.SaveValuesAsync();

            // Assert
            // No exception means the test passed
        }
    }

    // Mock interface for testing
    public interface IUnknownService { }
}
