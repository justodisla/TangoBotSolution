using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using TangoBot.App.App;
using TangoBot.App.Services;
using TangoBot.Core.Api2.Commons;
using TangoBot.Core.Domain.Components;
using TangoBot.Core.Domain.DTOs;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;
using Xunit;

namespace TangoBot.Tests.Services
{
    public class PositionServiceTests
    {
        
        private readonly PositionService _service;
        private IConfigurationProvider _configurationProvider;
        private string accountNumber;

        public PositionServiceTests()
        {
            Application application = new Application();
          
            _service = application.GetService<PositionService>();

            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>();

            accountNumber = _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_ACCOUNT_NUMBER);
        }

        [Fact]
        public void GetAccountPositions_ShouldReturnAccountPositionsDto()
        {
            // Arrange
            
            // Act
            var result = _service.GetAccountPositions(accountNumber);


            // Assert
            if (result.Positions.Count > 0)
            {
                Assert.NotNull(result);
                Assert.Single(result.Positions);
                Assert.Equal(accountNumber, result.Positions[0].AccountNumber);
                Assert.Equal("AAPL", result.Positions[0].Symbol);
                Assert.Equal("Stock", result.Positions[0].InstrumentType);
                Assert.Equal(100, result.Positions[0].Quantity);
                Assert.Equal(150.00, result.Positions[0].ClosePrice);
            }
            else { 
                Assert.True(result.Positions != null);
            }
        }
    }
}

