using HttpClientLib.TokenManagement;
using System.Threading.Tasks;
using TangoBot;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using Xunit;

namespace TangoBotTests
{
    public class TokenProviderTests
    {
        private readonly ITokenProvider? _tokenProvider;

        public TokenProviderTests()
        {
            StartUp.InitializeDI();
            //_tokenProvider = TangoBotServiceProvider.GetService<ITokenProvider>() as TokenProvider;
            _tokenProvider = TangoBotServiceProviderExp.GetSingletonService<ITokenProvider>() ?? throw new Exception("Token provider cannot be null");
        }

        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken_WhenTokenIsValid()
        {
            if (_tokenProvider == null)
            {
                Assert.True(false, "TokenProvider is null");
                return;
            }

            // Act
            var token = await _tokenProvider.GetValidTokenAsync();

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task GetStreamingTokenAsync_ReturnsStreamingToken_WhenSessionTokenIsValid()
        {
            if (_tokenProvider == null)
            {
                Assert.True(false, "TokenProvider is null");
                return;
            }
            // Act
            var result = await _tokenProvider.GetValidStreamingToken();

            // Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}
