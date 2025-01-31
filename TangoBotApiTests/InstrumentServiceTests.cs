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
    public class InstrumentServiceTests
    {

        private readonly InstrumentService _service;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string accountNumber;

        public InstrumentServiceTests()
        {
            Application application = new Application();

            _service = application.GetService<InstrumentService>();

            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>();

            accountNumber = _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_ACCOUNT_NUMBER);
        }

        [Fact]
        public void GetEquities_ShouldReturnInstrumentsDto()
        {
            // Arrange


            // Act
            try
            {
                var result = _service.GetInstrumentAsync().Result;
            }
            catch (Exception)
            {

                throw;
            }
            

            // Assert
           
        }
    }
}

