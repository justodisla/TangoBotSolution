using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.Core.Api2.Commons;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;

namespace TangoBot.App.App
{
    public partial class Application
    {
        public void SetupConfigurations()
        {
            IConfigurationProvider? configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>() ?? throw new Exception("Unable to access Configuration Provider");

            #region Environment configuration

            #region Production Configuration
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_URL, "https://api.tastyworks.comx");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_USER, "justodisla");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_PASSWORD, "?");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_ACCOUNT_NUMBER, "?");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_ID, "?");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_FIRST_NAME, "Justo");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_LAST_NAME, "Disla");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_EMAIL, "jdisla@gmail.com");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_MOBILE, "1-809-757-0665");
            configurationProvider.SetConfigurationValue(AppConstants.PRODUCTION_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL, "wss://streamer.tastyworks.com");
            #endregion

            #region Sandbox Configuration
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_URL, "https://api.cert.tastyworks.com");
            configurationProvider.SetConfigurationValue(AppConstants.SAND_BOX_USER, "tangobotsandboxuser");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_PASSWORD, "HyperBerserker?3000");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_ACCOUNT_NUMBER, "5WU34986");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_ID, "me");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_FIRST_NAME, "Sand");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_LAST_NAME, "Box");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_EMAIL, "jdisla@gmail.com");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_MOBILE, "1-809-757-0665");
            configurationProvider.SetConfigurationValue(AppConstants.SANDBOX_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL, "wss://streamer.cert.tastyworks.com");
            #endregion

            #region Switching environments
            configurationProvider.SetConfigurationValue(AppConstants.RUN_MODE, AppConstants.SAND_BOX_RUN_MODE);

            switch (configurationProvider.GetConfigurationValue(AppConstants.RUN_MODE))
            {
                case AppConstants.SAND_BOX_RUN_MODE:
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_USER, configurationProvider.GetConfigurationValue(AppConstants.SAND_BOX_USER));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_PASSWORD, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_PASSWORD));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_ACCOUNT_NUMBER, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_ACCOUNT_NUMBER));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_ID, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_ID));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_FIRST_NAME, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_FIRST_NAME));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_LAST_NAME, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_LAST_NAME));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_EMAIL, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_EMAIL));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_MOBILE, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_CUSTOMER_MOBILE));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_API_URL, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_URL));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_STREAMING_ACCOUNT_WEBSOCKET_URL, configurationProvider.GetConfigurationValue(AppConstants.SANDBOX_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL));
                    break;

                case AppConstants.PRODUCTION_RUN_MODE:
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_USER, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_USER));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_PASSWORD, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_PASSWORD));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_ACCOUNT_NUMBER, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_ACCOUNT_NUMBER));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_ID, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_ID));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_FIRST_NAME, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_FIRST_NAME));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_LAST_NAME, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_LAST_NAME));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_EMAIL, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_EMAIL));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_CUSTOMER_MOBILE, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_CUSTOMER_MOBILE));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_API_URL, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_URL));
                    configurationProvider.SetConfigurationValue(AppConstants.ACTIVE_STREAMING_ACCOUNT_WEBSOCKET_URL, configurationProvider.GetConfigurationValue(AppConstants.PRODUCTION_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL));
                    break;
            }
            #endregion

            #endregion

            #region Connections configurations
            configurationProvider.SetConfigurationValue(AppConstants.STREAMING_AUTH_TOKEN_ENDPOINT, "/api-quote-tokens");
            configurationProvider.SetConfigurationValue(AppConstants.DX_LINK_WS_URL, "wss://tasty-openapi-ws.dxfeed.com/realtime");
            configurationProvider.SetConfigurationValue(AppConstants.MAX_RETRY_ATTEMPTS, "3");
            configurationProvider.SetConfigurationValue(AppConstants.TOKEN_EXPIRATION_HOURS, "24");
            configurationProvider.SetConfigurationValue(AppConstants.JSON_CONTENT_TYPE, "application/json");

            configurationProvider.SetConfigurationValue(AppConstants.ALPHA_VANTAGE_API_KEY, "A5069SA46CZPGTHQ");
            #endregion


        }
    }
}
