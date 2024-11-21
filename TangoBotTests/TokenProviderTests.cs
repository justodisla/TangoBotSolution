using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientLib.TokenManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Moq.Protected;
using TangoBot;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using Xunit;
using Constants = TangoBotAPI.Toolkit.Constants;

namespace HttpClientLib.Tests.TokenManagement
{
    public class TokenProviderTests
    {
        private readonly TokenProvider _tokenProvider;

        public TokenProviderTests()
        {
            StartUp.InitializeDI();
            _tokenProvider = TangoBotServiceProvider.GetService<ITokenProvider>() as TokenProvider;
        }

        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken_WhenTokenIsValid()
        {
            // Act
            var token = await _tokenProvider.GetValidTokenAsync();

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task GetStreamingTokenAsync_ReturnsStreamingToken_WhenSessionTokenIsValid()
        {
            
            TangoBotServiceProvider.GetService<IConfigurationProvider>().SetConfigurationValue(Constants.STREAMING_AUTH_TOKEN, "");

            // Act
            var result = await _tokenProvider.GetValidStreamingToken();

            // Assert
            //Assert.Equal(streamingToken, result);
        }
    }
}
   
