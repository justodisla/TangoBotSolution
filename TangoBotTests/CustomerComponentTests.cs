using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpClientLib.TokenManagement;
using Moq;
using Moq.Protected;
using TangoBot.HttpClientLib;
using Xunit;

namespace TangoBotTests
{
    public class CustomerComponentTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;
        private readonly CustomerComponent _customerComponent;

        public CustomerComponentTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tokenProvider = new TokenProvider(_httpClient);
            _customerComponent = new CustomerComponent(_httpClient, _tokenProvider);
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
        public async Task GetCustomerInfoAsync_Returns_CustomerInfo()
        {
            // Arrange
            string jsonResponse = "{\"data\":{\"id\":\"me\",\"first-name\":\"John\",\"last-name\":\"Doe\",\"email\":\"john.doe@example.com\",\"mobile-phone-number\":\"123-456-7890\",\"permitted-account-types\":[]}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _customerComponent.GetCustomerInfoAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("me", result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john.doe@example.com", result.Email);
        }

        [Fact]
        public async Task GetCustomerAccountsAsync_Returns_Accounts()
        {
            // Arrange
            string jsonResponse = "{\"data\":{\"items\":[{\"account\":{\"account-number\":\"123456\",\"account-type-name\":\"Individual\",\"created-at\":\"2024-11-12T12:28:44.126+00:00\",\"day-trader-status\":false,\"investment-objective\":\"SPECULATION\"}}]}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _customerComponent.GetCustomerAccountsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("123456", result[0].AccountNumber);
            Assert.Equal("Individual", result[0].AccountTypeName);
            Assert.False(result[0].DayTraderStatus);
            Assert.Equal("SPECULATION", result[0].InvestmentObjective);
        }

        [Fact]
        public async Task GetAccountDetailsAsync_Returns_AccountDetails()
        {
            // Arrange
            string accountNumber = "123456";
            string jsonResponse = "{\"data\":{\"account-number\":\"123456\",\"account-type-name\":\"Individual\",\"day-trader-status\":false,\"investment-objective\":\"SPECULATION\"}}";
            SetupHttpResponse(jsonResponse);

            // Act
            var result = await _customerComponent.GetAccountDetailsAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.AccountNumber);
            Assert.Equal("Individual", result.AccountTypeName);
            Assert.False(result.DayTraderStatus);
            Assert.Equal("SPECULATION", result.InvestmentObjective);
        }
    }
}
