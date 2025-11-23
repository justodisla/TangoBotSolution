using TangoBot;
using TangoBot.API.Configuration;
using TangoBot.API.Toolkit;
using TangoBot.DependecyInjection;

namespace TangoBotTests
{
    public class AccountStatusComponentTests
    {
        private readonly AccountStatusComponent _accountStatusComponent;
        private readonly IConfigurationProvider _configurationProvider;

        public AccountStatusComponentTests()
        {
            StartUp.InitializeDI();

            _configurationProvider = TangoBotServiceLocator.GetSingletonService<IConfigurationProvider>()
                ?? throw new Exception("ConfigurationProvider is null");

            var activeAccount = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);
            var sandboxAccountNumber = _configurationProvider.GetConfigurationValue(Constants.SANDBOX_ACCOUNT_NUMBER);

            if (activeAccount != sandboxAccountNumber)
                throw new Exception("Wrong account number used");

            _accountStatusComponent = TangoBotServiceLocator.GetSingletonService<AccountStatusComponent>()
                ?? throw new Exception("AccountStatusComponent is null");
        }

        [Fact]
        public async Task GetTradingStatusAsync_ReturnsTradingStatus_WhenResponseIsSuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER);

            // Act
            var result = await _accountStatusComponent.GetTradingStatusAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountNumber, result.AccountNumber);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task GetTradingStatusAsync_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var accountNumber = _configurationProvider.GetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER) + "X";

            // Act
            var result = await _accountStatusComponent.GetTradingStatusAsync(accountNumber);

            // Assert
            Assert.Null(result);
        }
    }
}
