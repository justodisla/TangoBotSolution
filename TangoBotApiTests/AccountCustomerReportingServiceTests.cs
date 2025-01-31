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
        public void GetAccountBalances_ShouldReturnAccountBalancesDto()
        {
            // Arrange
            double cashBalance = 1000000;

            // Act
            var result = _service.GetAccountBalances(accountNumber);

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
            var result = _service.GetAccountSnapShots(accountNumber);

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

        [Fact]
        public void GetCustomerAccounts_ShouldReturnCustomerAccountsDto()
        {
            // Arrange
            const string accountTypeName = "Individual";
            // Act
            var result = _service.GetCustomerAccounts();
            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(accountTypeName, result.Items[0].Account.AccountTypeName);
        }

        [Fact]
        public void GetAccountStatus_ShouldReturnAccountStatusDto()
        {
            // Arrange
            const string accountStatus = "Active";
            // Act
            var result = _service.GetAccountStatus(accountNumber);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountNumber, result.AccountNumber);
            Assert.True(result.IsRollTheDayForwardEnabled);
        }

        [Fact]
        public void GetAccountTransactions_ShouldReturnAccountTransactionsDto()
        {
            // Arrange
            const string transactionType = "Buy";
            // Act
            var result = _service.GetAccountTransactions(accountNumber);
            // Assert
            Assert.NotNull(result);
            //Assert.Single(result.Items);
            //Assert.Equal(transactionType, result.Items[0].TransactionType);
        }
    }
}

