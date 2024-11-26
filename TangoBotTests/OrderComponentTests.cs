using HttpClientLib.AccountApi;
using HttpClientLib.OrderApi;
using HttpClientLib.OrderApi.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBot;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Toolkit;
using Xunit;

namespace TangoBotTests
{
    public class OrderComponentTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly IOrderComponent<Order> _orderComponent;
        private readonly string _accountNumber;
        private readonly List<int> _createdOrderIds;

        private readonly IConfigurationProvider _configurationProvider;

        public OrderComponentTests()
        {
            StartUp.InitializeDI();

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _orderComponent = TangoBotServiceProviderExp.GetSingletonService<IOrderComponent<Order>>() 
                ?? throw new Exception("Account component is null.");
            
            _configurationProvider = TangoBotServiceProviderExp.GetSingletonService<IConfigurationProvider>() ?? throw new Exception("Configuration provider is null");

            _accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);

            _createdOrderIds = new List<int>();

            // Cancel all cancellable orders at the start
            CancelAllCancellableOrdersAsync().Wait();
        }

        private async Task CancelAllCancellableOrdersAsync()
        {
            var orders = await _orderComponent.GetAccountOrdersAsync(_accountNumber);
            if (orders != null)
            {
                foreach (var order in orders.Where(o => o.Cancellable))
                {
                    try
                    {
                        await _orderComponent.CancelOrderByIdAsync(_accountNumber, order.Id);
                    }
                    catch
                    {
                        // Log or handle the exception if needed
                    }
                }
            }
        }

        [Fact]
        public async Task GetAccountOrdersAsync_ReturnsOrders_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;

            // Act
            var result = await _orderComponent.GetAccountOrdersAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
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

            var postOrderResult = await _orderComponent.PostEquityOrder(accountNumber, orderRequest, false);
            var orderId = postOrderResult.Data.Order.Id;
            _createdOrderIds.Add(orderId);

            // Act
            var orderReport = await _orderComponent.GetOrderByIdAsync(accountNumber, orderId);

            // Assert
            Assert.Equal(orderId, orderReport.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await _orderComponent.GetOrderByIdAsync(accountNumber, 123456));
        }

        [Fact]
        public async Task PostEquityOrderDryRun_ReturnsOrderPostReport_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
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
        }

        [Fact]
        public async Task PostEquityOrder_ReturnsOrderPostReport_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
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
            _createdOrderIds.Add(result.Data.Order.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostEquityOrder_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var orderRequest = new OrderRequest { /* Initialize properties */ };

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _orderComponent.PostEquityOrder(accountNumber, orderRequest));
        }

        [Fact]
        public async Task CancelOrderByIdAsync_ReturnsOrder_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
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

            var postOrderResult = await _orderComponent.PostEquityOrder(accountNumber, orderRequest, false);
            var orderId = postOrderResult.Data.Order.Id;
            _createdOrderIds.Add(orderId);

            // Act
            var result = await _orderComponent.CancelOrderByIdAsync(accountNumber, orderId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Status.ToLower().Contains("cancel"));
        }

        [Fact]
        public async Task CancelOrderByIdAsync_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _accountNumber;
            var orderId = 99999;

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _orderComponent.CancelOrderByIdAsync(accountNumber, orderId));
        }

        public void Dispose()
        {
            // Cleanup all created orders
            foreach (var orderId in _createdOrderIds)
            {
                try
                {
                    _orderComponent.CancelOrderByIdAsync(_accountNumber, orderId).Wait();
                }
                catch
                {
                    // Log or handle the exception if needed
                }
            }
        }
    }
}
