using System.Threading.Tasks;
using Xunit;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Auth;

namespace TangoBotApi.Tests
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _authService = ServiceLocator.GetSingletonService<IAuthService>();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnTrueForValidCredentials()
        {
            // Arrange
            var username = "tangobotsandboxuser";
            var password = "HyperBerserker?3000";

            // Act
            var result = await _authService.AuthenticateAsync(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnFalseForInvalidCredentials()
        {
            // Arrange
            var username = "user1";
            var password = "wrongpassword";

            // Act
            var result = await _authService.AuthenticateAsync(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GenerateTokenAsync_ShouldReturnTokenForAuthenticatedUser()
        {
            // Arrange
            var username = "user1";
            var password = "password1";
            await _authService.AuthenticateAsync(username, password);

            // Act
            var token = await _authService.GenerateTokenAsync(username);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task AuthorizeAsync_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var username = "user1";
            var password = "password1";
            await _authService.AuthenticateAsync(username, password);
            var token = await _authService.GenerateTokenAsync(username);

            // Act
            var result = await _authService.AuthorizeAsync(token, "anyrole");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthorizeAsync_ShouldReturnFalseForInvalidToken()
        {
            // Act
            var result = await _authService.AuthorizeAsync("invalidtoken", "anyrole");

            // Assert
            Assert.False(result);
        }
    }
}
