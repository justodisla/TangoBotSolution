using HttpClientLib.OrderApi;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBot;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Toolkit;
using Xunit;

namespace HttpClientLib.Tests.OrderApi
{
    public class OrderComponentTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly OrderComponent _orderComponent;
        private string _accountNumber;

        public OrderComponentTests()
        {

            StartUp.InitializeDI();

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _orderComponent = new OrderComponent();

            _accountNumber = TangoBotServiceProvider.GetService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);
        }

        [Fact]
        public async Task GetAccountOrdersAsync_ReturnsOrders_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            //var responseContent = "[{\"orderId\": 1, \"status\": \"filled\"}]";


            // Act
            var result = await _orderComponent.GetAccountOrdersAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            //Assert.Single(result);
            //Assert.Equal(1, result[0].OrderId);
            //Assert.Equal("filled", result[0].Status);
        }

        [Fact]
        public async Task GetAccountOrdersAsync_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber + "3";


            // Act
            var result = await _orderComponent.GetAccountOrdersAsync(accountNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var responseContent = "{\"orderId\": 1, \"status\": \"filled\"}";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            var orderRequest = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                {
                    new LegRequest
                    {
                        Symbol = "AAPL",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            var postOrderDryRun = await _orderComponent.PostEquityOrder(accountNumber, orderRequest);

            // Act & Assert
            var postOrderResult = await _orderComponent.PostEquityOrder(accountNumber, orderRequest, false);

            var orderId = postOrderResult.Data.Order.Id;

            var orderReport = await _orderComponent.GetOrderByIdAsync(accountNumber, orderId);

            Assert.Equal(orderId, orderReport.Id);

            //Let's cancel the order
            var cancelResult = await _orderComponent.CancelOrderByIdAsync(accountNumber, orderId);

            Assert.Equal("Cancel Requested", cancelResult.Status);

            // Attempt to retrieve the cancelled order
            var cancelledOrder = await _orderComponent.GetOrderByIdAsync(accountNumber, orderId);

            Assert.Equal("Cancelled", cancelledOrder.Status);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await _orderComponent.GetOrderByIdAsync(accountNumber, 123456));
        }

        [Fact]
        public async Task PostEquityOrderDryRun_ReturnsOrderPostReport_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            //var orderRequest = new OrderRequest { TimeInForce };
            var responseContent = "{\"orderId\": 1, \"status\": \"filled\"}";

            var orderRequest = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                {
                    new LegRequest
                    {
                        Symbol = "AAPL",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            // Act
            var result = await _orderComponent.PostEquityOrder(accountNumber, orderRequest);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(1, result.OrderId);
            //Assert.Equal("filled", result.Status);
        }

        [Fact]
        public async Task PostEquityOrder_ReturnsOrderPostReport_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            //var orderRequest = new OrderRequest { TimeInForce };
            var responseContent = "{\"orderId\": 1, \"status\": \"filled\"}";

            var orderRequest = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                {
                    new LegRequest
                    {
                        Symbol = "AAPL",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            // Act
            var result = await _orderComponent.PostEquityOrder(accountNumber, orderRequest, false);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(1, result.OrderId);
            //Assert.Equal("filled", result.Status);
        }

        [Fact]
        public async Task PostEquityOrder_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var orderRequest = new OrderRequest { /* Initialize properties */ };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _orderComponent.PostEquityOrder(accountNumber, orderRequest));
        }

        [Fact]
        public async Task CancelOrderByIdAsync_ReturnsOrder_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var responseContent = "{\"orderId\": 1, \"status\": \"canceled\"}";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            var orderRequest = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                 {
                    new LegRequest
                    {
                        Symbol = "SPY",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            // Act
            var postOrderResult = await _orderComponent.PostEquityOrder(accountNumber, orderRequest, false);
            var orderId = postOrderResult.Data.Order.Id;
            //postOrderResult.Data.("orderId", out var orderIdValue);

            var result = await _orderComponent.CancelOrderByIdAsync(accountNumber, orderId);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(1, result.OrderId);
            Assert.Equal("Cancel Requested", result.Status);
        }

        [Fact]
        public async Task CancelOrderByIdAsync_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var orderId = 99999;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _orderComponent.CancelOrderByIdAsync(accountNumber, orderId));
        }
    }
}
