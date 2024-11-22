using HttpClientLib.AccountApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBot;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Toolkit;
using Xunit;

namespace HttpClientLib.Tests.AccountApi
{
    public class AccountComponentTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly AccountComponent _accountComponent;
        private readonly IConfigurationProvider _configurationProvider;


        public AccountComponentTests()
        {

            StartUp.InitializeDI();

            //_httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            //_httpClient = TangoBotServiceProvider.GetService<HttpClient>();
            _accountComponent = TangoBotServiceProvider.GetService<AccountComponent>();

            _configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();

        }

        [Fact]
        public async Task GetAccountBalancesAsync_ReturnsAccountBalances_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);

            // Act
            var result = await _accountComponent.GetAccountBalancesAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("cash-balance"));
            Assert.Equal("1000000.0", result["cash-balance"]);
        }

        [Fact]
        public async Task GetAccountBalancesAsync_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER) + "X";

            // Act
            var result = await _accountComponent.GetAccountBalancesAsync(accountNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetBalanceSnapshotAsync_ReturnsBalanceSnapshots_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);
            var responseContent = "[{\"snapshot\": 1000.0}]";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            // Act
            var result = await _accountComponent.GetBalanceSnapshotAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetBalanceSnapshotAsync_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER) + "X";

            // Act
            var result = await _accountComponent.GetBalanceSnapshotAsync(accountNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAccountPositionsAsync_ReturnsAccountPositions_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);
            var responseContent = "[{\"position\": 1000.0}]";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            //_httpMessageHandlerMock
            //    .Setup(m => m.Send(It.IsAny<HttpRequestMessage>()))
            //    .Returns(responseMessage);

            // Act
            var result = await _accountComponent.GetAccountPositionsAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result[0].ContainsKey("position"));
            Assert.Equal(1000.0, result[0]["position"]);
        }

        [Fact]
        public async Task GetAccountPositionsAsync_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = "123456";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            //_httpMessageHandlerMock
            //    .Setup(m => m.Send(It.IsAny<HttpRequestMessage>()))
            //    .Returns(responseMessage);

            // Act
            var result = await _accountComponent.GetAccountPositionsAsync(accountNumber);

            // Assert
            Assert.Null(result);
        }
    }
}
