using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpClientLib.AccountApi;
using HttpClientLib.TokenProviding;
using Moq;
using Moq.Protected;
using TangoBot.HttpClientLib;
using Xunit;

namespace TangoBotTests
{
    public class AccountComponentTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;
        private readonly AccountComponent _accountComponent;

        public AccountComponentTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tokenProvider = new TokenProvider(_httpClient);
            _accountComponent = new AccountComponent(_httpClient, _tokenProvider);
        }

        private void SetupHttpResponse(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(jsonResponse)
                });
        }

        [Fact]
        public async Task GetAccountBalanceAsync_Returns_AccountBalance()
        {
            // Arrange
            string accountNumber = "123456";
            string jsonResponse = "{\"data\":{\"account-number\":\"123456\",\"available-trading-funds\":\"5000.0\",\"cash-balance\":\"10000.0\"}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _accountComponent.GetAccountBalanceAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.AccountNumber);
            Assert.Equal("5000.0", result.AvailableTradingFunds);
            Assert.Equal("10000.0", result.CashBalance);
        }

        [Fact]
        public async Task GetBalanceSnapshotsAsync_Returns_Snapshots()
        {
            // Arrange
            string accountNumber = "123456";
            string jsonResponse = "{\"data\":{\"items\":[{\"snapshot-date\":\"2024-11-12\",\"available-trading-funds\":\"5000.0\",\"cash-balance\":\"10000.0\"}]}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _accountComponent.GetBalanceSnapshotsAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.items);
            Assert.Equal("2024-11-12", result.items[0].SnapshotDate);
            Assert.Equal("5000.0", result.items[0].AvailableTradingFunds);
            Assert.Equal("10000.0", result.items[0].CashBalance);
        }

        [Fact]
        public async Task GetPositionsAsync_Returns_Positions()
        {
            // Arrange
            string accountNumber = "123456";
            string jsonResponse = "{\"data\":{\"items\":[{\"account-number\":\"123456\",\"symbol\":\"AAPL\",\"quantity\":10,\"average-open-price\":\"150.0\"}]}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _accountComponent.GetPositionsAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.items);
            Assert.Equal("123456", result.items[0].AccountNumber);
            Assert.Equal("AAPL", result.items[0].Symbol);
            Assert.Equal(10, result.items[0].Quantity);
            Assert.Equal("150.0", result.items[0].AverageOpenPrice);
        }
    }
}
