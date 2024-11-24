using HttpClientLib;
using HttpClientLib.AccountApi;
using HttpClientLib.CustomerApi;
using HttpClientLib.InstrumentApi;
using HttpClientLib.OrderApi;
using HttpClientLib.OrderApi.Models;
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

namespace TangoBotTests
{
    public class AccountComponentTests
    {
        private readonly AccountComponent _accountComponent;
        private readonly IConfigurationProvider _configurationProvider;

        public AccountComponentTests()
        {


            StartUp.InitializeDI();

            _configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>() ?? throw new Exception("ConfigurationProvider is null");


            var activeAccount = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);
            var sandboxAccountNumber = _configurationProvider.GetConfigurationValue(Constants.SANDBOX_ACCOUNT_NUMBER);

            if (activeAccount != sandboxAccountNumber)
                throw new Exception("Wrong account number used");

            //_httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            //_httpClient = TangoBotServiceProvider.GetService<HttpClient>();
            _accountComponent = TangoBotServiceProvider.GetService<AccountComponent>() ?? throw new Exception("AccountComponent is null");

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
            var _orderComponent = TangoBotServiceProvider.GetService<OrderComponent>() ?? throw new Exception("OrderComponent null");
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);

            //var msc = TangoBotServiceProvider.GetService<MarketStatusChecker>();
            //bool isMarketOpened = await msc.IsMarketOpenAsync();

            MarketStatusChecker? marketStatusChecker = TangoBotServiceProvider.GetService<MarketStatusChecker>() ?? throw new Exception("Unable to load MarketStatusChecker");
            if (!await marketStatusChecker.IsMarketOpenAsync())
            {
                Assert.True(true);
                return;
            }

            var orderRequest = new OrderRequest
            {
                OrderType = "Market",
                Price = null,
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
            var newDryRunOrder = await _orderComponent.PostEquityOrder(accountNumber, orderRequest);

            //var newOrder = await _orderComponent.PostEquityOrder(accountNumber, orderRequest);

            //Thread.Sleep(5000);

            // Act
            var result = await _accountComponent.GetAccountPositionsAsync(accountNumber);


            //_orderComponent.CancelOrderByIdAsync(accountNumber, newOrder.Data.Order.Id);

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
