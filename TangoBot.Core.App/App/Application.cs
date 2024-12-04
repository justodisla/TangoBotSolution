using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TangoBot.App.Services;
using TangoBot.Core.Api2.Commons;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;

namespace TangoBot.App.App
{
    public partial class Application
    {
        private static bool _isInitialized = false;
        private static bool _isTerminated = false;

        public enum ServiceType
        {
            AccountService,
            InstrumentService,
            OrderService,
            MarketDataService
        }

        public Application()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;

            //Initialize configurations
            SetupConfigurations();

            //Initialize services
            RegisterService<AccountReportingService>(new AccountReportingService());

            //Initialize repositories

            _isInitialized = true;
        }

        public void Terminate()
        {
            if (_isTerminated)
                return;

            // Perform cleanup tasks here

            _isTerminated = true;
        }
    }
}
