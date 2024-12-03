using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;
using Xunit;

namespace TangoBotApi.Tests
{
    public class ConfigurationProviderTests
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ConfigurationProviderTests()
        {
            ServiceLocator.Initialize();
            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>();
        }

        [Fact]
        public void GetConfigurationValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";
            _configurationProvider.SetConfigurationValue(key, value);

            // Act
            var result = _configurationProvider.GetConfigurationValue(key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void GetAllConfigurationValues_ShouldReturnAllValues()
        {
            _configurationProvider.ResetConfiguration();

            // Arrange
            var key1 = "TestKey1";
            var value1 = "TestValue1";
            var key2 = "TestKey2";
            var value2 = "TestValue2";
            _configurationProvider.SetConfigurationValue(key1, value1);
            _configurationProvider.SetConfigurationValue(key2, value2);

            // Act
            var result = _configurationProvider.GetAllConfigurationValues();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(value1, result[key1]);
            Assert.Equal(value2, result[key2]);

        }

        [Fact]
        public void SetConfigurationValue_ShouldStoreValue()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";

            // Act
            _configurationProvider.SetConfigurationValue(key, value);
            var result = _configurationProvider.GetConfigurationValue(key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task SaveConfigurationAsync_ShouldCompleteWithoutException()
        {
            // Act
            var exception = await Record.ExceptionAsync(() => _configurationProvider.SaveConfigurationAsync());

            // Assert
            Assert.Null(exception);
        }
    }
}
