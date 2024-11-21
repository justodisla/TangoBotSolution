using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientLib.TokenManagement;
using Microsoft.Extensions.DependencyInjection;
using TangoBot;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using Xunit;

namespace HttpClientLib.Tests.TokenManagement
{
    public class TokenProviderTests
    {
        private readonly TokenProvider _tokenProvider;

        public TokenProviderTests()
        {

            StartUp.InitializeDI();

            // Initialize the service provider
            TangoBotServiceProvider.Initialize();

            // Register services
            TangoBotServiceProvider.AddSingletonService<IConfigurationProvider>(new ConfigurationProvider());
            TangoBotServiceProvider.AddSingletonService(new HttpClient());
            //TangoBotServiceProvider.AddSingletonService<ITokenProvider>(new TokenProvider());

            // Resolve the TokenProvider
            _tokenProvider = TangoBotServiceProvider.GetService<ITokenProvider>() as TokenProvider;
        }

        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken_WhenTokenIsValid()
        {
            // Arrange
            var configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();
            var validToken = "valid_token";
            configurationProvider.SetConfigurationValue("api_session_token", validToken);

            // Act
            var token = await _tokenProvider.GetValidTokenAsync();

            // Assert
            Assert.Equal(validToken, token);
        }

        [Fact]
        public async Task GetValidTokenAsync_RequestsNewToken_WhenTokenIsInvalid()
        {
            // Arrange
            var configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();
            var invalidToken = "invalid_token";
            configurationProvider.SetConfigurationValue("api_session_token", invalidToken);

            // Act
            var token = await _tokenProvider.GetValidTokenAsync();

            // Assert
            Assert.NotEqual(invalidToken, token);
            Assert.False(string.IsNullOrEmpty(token));
        }
    }

    // Dummy implementation of IConfigurationProvider for testing purposes
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, string> _configurations = new();

        public string GetConfigurationValue(string key)
        {
            _configurations.TryGetValue(key, out var value);
            return value;
        }

        public IDictionary<string, string> GetAllConfigurationValues()
        {
            return _configurations;
        }

        public void SetConfigurationValue(string key, string value)
        {
            _configurations[key] = value;
        }

        public Task SaveConfigurationAsync()
        {
            return Task.CompletedTask;
        }
    }
}
