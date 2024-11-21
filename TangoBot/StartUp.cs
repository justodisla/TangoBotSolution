﻿using HttpClientLib.AccountApi;
using HttpClientLib.CustomerApi;
using HttpClientLib.InstrumentApi;
using HttpClientLib.OrderApi;
using HttpClientLib.TokenManagement;
using TangoBotAPI.Configuration;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using TangoBotServiceProvider = TangoBotAPI.DI.TangoBotServiceProvider;

namespace TangoBot
{
    public class StartUp
    {
        /// <summary>
        /// Initializes the Dependency Injection container.
        /// </summary>
        public static void InitializeDI()
        {
            TangoBotServiceProvider.AddSingletonService<IConfigurationProvider>(new ConfigurationProvider());

            SetupServices();
            SetupConfigurations();
        }

        /// <summary>
        /// Sets up the services required by the application.
        /// </summary>
        private static void SetupServices()
        {
            TangoBotServiceProvider.AddSingletonService<HttpClient>(new HttpClient());
            TangoBotServiceProvider.AddSingletonService<ITokenProvider>(new TokenProvider());
            TangoBotServiceProvider.AddSingletonService<AccountComponent>(new AccountComponent());
            TangoBotServiceProvider.AddSingletonService<CustomerComponent>(new CustomerComponent());
            TangoBotServiceProvider.AddSingletonService<OrderComponent>(new OrderComponent());
            TangoBotServiceProvider.AddSingletonService<InstrumentComponent>(new InstrumentComponent());
        }

        /// <summary>
        /// Sets up the configurations required by the application.
        /// </summary>
        private static void SetupConfigurations()
        {
            IConfigurationProvider? configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();

            if (configurationProvider == null)
            {
                throw new Exception("Unable to access Configuration Provider");
            }

            #region Environment configuration

            #region Production Configuration
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_URL, "https://api.tastyworks.com");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_USER, "justodisla");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_PASSWORD, "?");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_ACCOUNT_NUMBER, "?");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_CUSTOMER_ID, "?");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_CUSTOMER_FIRST_NAME, "Justo");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_CUSTOMER_LAST_NAME, "Disla");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_CUSTOMER_EMAIL, "jdisla@gmail.com");
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_CUSTOMER_MOBILE, "1-809-757-0665");
            #endregion

            #region Sandbox Configuration
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_URL, "https://api.cert.tastyworks.com");
            configurationProvider.SetConfigurationValue(Constants.SAND_BOX_USER, "tangobotsandboxuser");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_PASSWORD, "TTTangoBotSandBoxPass");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_ACCOUNT_NUMBER, "5WU34986");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_ID, "me");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_FIRST_NAME, "Sand");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_LAST_NAME, "Box");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_EMAIL, "jdisla@gmail.com");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_MOBILE, "1-809-757-0665");
            #endregion

            #region Switching environments
            configurationProvider.SetConfigurationValue(Constants.RUN_MODE, Constants.SAND_BOX_RUN_MODE);

            switch (Constants.RUN_MODE)
            {
                case Constants.SAND_BOX_RUN_MODE:
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_USER, Constants.SAND_BOX_USER);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_PASSWORD, Constants.SANDBOX_PASSWORD);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER, Constants.SANDBOX_ACCOUNT_NUMBER);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_ID, Constants.SANDBOX_CUSTOMER_ID);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_FIRST_NAME, Constants.SANDBOX_CUSTOMER_FIRST_NAME);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_LAST_NAME, Constants.SANDBOX_CUSTOMER_LAST_NAME);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_EMAIL, Constants.SANDBOX_CUSTOMER_EMAIL);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_MOBILE, Constants.SANDBOX_CUSTOMER_MOBILE);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_API_URL, Constants.SANDBOX_URL);
                    break;

                case Constants.PRODUCTION_RUN_MODE:
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_USER, Constants.PRODUCTION_USER);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_PASSWORD, Constants.PRODUCTION_PASSWORD);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER, Constants.PRODUCTION_ACCOUNT_NUMBER);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_ID, Constants.PRODUCTION_CUSTOMER_ID);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_FIRST_NAME, Constants.PRODUCTION_CUSTOMER_FIRST_NAME);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_LAST_NAME, Constants.PRODUCTION_CUSTOMER_LAST_NAME);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_EMAIL, Constants.PRODUCTION_CUSTOMER_EMAIL);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_MOBILE, Constants.PRODUCTION_CUSTOMER_MOBILE);
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_API_URL, Constants.PRODUCTION_URL);
                    break;
            }
            #endregion

            #endregion

            #region Connections configurations
            configurationProvider.SetConfigurationValue(Constants.STREAMING_AUTH_TOKEN_ENDPOINT, "/api-quote-tokens");
            configurationProvider.SetConfigurationValue(Constants.DX_LINK_WS_URL, "wss://tasty-openapi-ws.dxfeed.com/realtime");
            configurationProvider.SetConfigurationValue(Constants.MAX_RETRY_ATTEMPTS, "3");
            configurationProvider.SetConfigurationValue(Constants.TOKEN_EXPIRATION_HOURS, "24");
            configurationProvider.SetConfigurationValue(Constants.JSON_CONTENT_TYPE, "application/json");
            #endregion
        }
    }
}
