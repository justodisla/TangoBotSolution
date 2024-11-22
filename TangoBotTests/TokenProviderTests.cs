using HttpClientLib.TokenManagement;
using System.Threading.Tasks;
using TangoBot;
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
            // Act
            var result = await _tokenProvider.GetValidStreamingToken();

            // Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}