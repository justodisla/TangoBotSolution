using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TangoBot.App.App;
using TangoBot.App.DTOs;
using TangoBot.App.Services;
using TangoBot.Core.Api2.Commons;
using TangoBot.Core.Domain.Services;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;
using Xunit;

namespace TangoBot.Tests.Services
{
    public class AccountCustomerReportingServiceTests
    {
        private readonly Mock<TTAccountCustomerComponent> _mockAccountComponent;
        private readonly AccountCustomerReportingService _service;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string accountNumber;

        public AccountCustomerReportingServiceTests()
        {
            Application application = new Application();

            _mockAccountComponent = new Mock<TTAccountCustomerComponent>();
            _service = application.GetService<AccountCustomerReportingService>();

            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>();

            accountNumber = _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_ACCOUNT_NUMBER);

        }

        [Fact]
        public void GetAccountBalance_ShouldReturnAccountBalanceDto()
        {
            // Arrange
            double cashBalance = 1000000;

            // Act
            var result = _service.GetAccountBalance(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountNumber, result.AccountNumber);
            Assert.Equal(cashBalance, result.CashBalance);
        }

        [Fact]
        public void GetAccount_ShouldReturnAccountDto()
        {
            // Arrange
            string accountTypeName = "Individual";

            // Act
            var result = _service.GetAccount(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountNumber, result.AccountNumber);

            Assert.Equal(accountTypeName, result.AccountTypeName);
        }

        [Fact]
        public void GetAccountSnapShot_ShouldReturnAccountSnapShotDto()
        {
            // Arrange
            double cashBalance = 1000000;

            // Act
            var result = _service.GetAccountSnapShot(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(accountNumber, result.Items[0].AccountNumber);
            Assert.Equal(cashBalance, result.Items[0].CashBalance);
        }

        [Fact]
        public void GetCustomer_ShouldReturnCustomerDto()
        {
            // Arrange
            const string firstName = "Sand";
            const string LastName = "Box";

            // Act
            var result = _service.GetCustomer();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("me", result.Id);

            Assert.Equal(firstName, result.FirstName);

            Assert.Equal(LastName, result.LastName);
        }
    }
}

