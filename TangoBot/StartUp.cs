using HttpClientLib;
using HttpClientLib.AccountApi;
using HttpClientLib.AccountApi.Observer;
using HttpClientLib.CustomerApi;
using HttpClientLib.InstrumentApi;
using HttpClientLib.OrderApi;
using HttpClientLib.OrderApi.Observer;
using HttpClientLib.TokenManagement;
using TangoBotAPI.Configuration;
using TangoBotAPI.Streaming;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using TangoBotStreaming.Observables;
using TangoBotStreaming.Services;
using TangoBotServiceProvider = TangoBotAPI.DI.TangoBotServiceProvider;

namespace TangoBot
{
    public class StartUp
    {
        private static bool IsInitialized { get; set; }

        /// <summary>
        /// Initializes the Dependency Injection container.
        /// </summary>
        public static void InitializeDI()
        {

            if (IsInitialized)
            {
                return;
            }

            TangoBotServiceProvider.AddService<IConfigurationProvider>(provider => new ConfigurationProvider());

            SetupConfigurations();

            SetupServices();

            IsInitialized = true;
        }

        /// <summary>
        /// Sets up the services required by the application.
        /// </summary>
        private static void SetupServices()
        {
            TangoBotServiceProvider.AddService<HttpClient>(provider => new HttpClient());
            TangoBotServiceProvider.AddService<ITokenProvider>(provider => new TokenProvider());
            TangoBotServiceProvider.AddService<AccountComponent>(provider => new AccountComponent());
            TangoBotServiceProvider.AddService<CustomerComponent>(provider => new CustomerComponent());
            TangoBotServiceProvider.AddService<OrderComponent>(provider => new OrderComponent());
            TangoBotServiceProvider.AddService<InstrumentComponent>(provider => new InstrumentComponent());
            TangoBotServiceProvider.AddService<MarketStatusChecker>(provider => new MarketStatusChecker());
            //TangoBotServiceProvider.AddSingletonService<IStreamService<?>>(new StreamingService());

            //Configure streaming service
            TangoBotServiceProvider.AddService<TangoBotAPI.Streaming.IStreamingService>(provider => new StreamingService(), typeof(StreamingService).Name);
            var _streamingService = TangoBotServiceProvider.GetSingletonService<IStreamingService>(typeof(StreamingService).Name);
            ((IObservable<HistoricDataReceivedEvent>)_streamingService).Subscribe(new HistoryDataStreamObserver());
            var hc = _streamingService.GetHashCode();

            //Subscribe to the HttpResponseEvent
            ((IObservable<HttpResponseEvent>)TangoBotServiceProvider.GetService<AccountComponent>()).Subscribe(new OrderObserver());
            ((IObservable<HttpResponseEvent>)TangoBotServiceProvider.GetService<AccountComponent>()).Subscribe(new AccountObserver());
            ((IObservable<HttpResponseEvent>)TangoBotServiceProvider.GetService<CustomerComponent>()).Subscribe(new OrderObserver());
            ((IObservable<HttpResponseEvent>)TangoBotServiceProvider.GetService<OrderComponent>()).Subscribe(new OrderObserver());
            ((IObservable<HttpResponseEvent>)TangoBotServiceProvider.GetService<InstrumentComponent>()).Subscribe(new OrderObserver());
        }

        /// <summary>
        /// Sets up the configurations required by the application.
        /// </summary>
        private static void SetupConfigurations()
        {
            IConfigurationProvider? configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>() ?? throw new Exception("Unable to access Configuration Provider");

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
            configurationProvider.SetConfigurationValue(Constants.PRODUCTION_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL, "wss://streamer.tastyworks.com");
            #endregion

            #region Sandbox Configuration
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_URL, "https://api.cert.tastyworks.com");
            configurationProvider.SetConfigurationValue(Constants.SAND_BOX_USER, "tangobotsandboxuser");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_PASSWORD, "HyperBerserker?3000");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_ACCOUNT_NUMBER, "5WU34986");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_ID, "me");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_FIRST_NAME, "Sand");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_LAST_NAME, "Box");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_EMAIL, "jdisla@gmail.com");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_CUSTOMER_MOBILE, "1-809-757-0665");
            configurationProvider.SetConfigurationValue(Constants.SANDBOX_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL, "wss://streamer.cert.tastyworks.com");
            #endregion

            #region Switching environments
            configurationProvider.SetConfigurationValue(Constants.RUN_MODE, Constants.SAND_BOX_RUN_MODE);

            switch (configurationProvider.GetConfigurationValue(Constants.RUN_MODE))
            {
                case Constants.SAND_BOX_RUN_MODE:
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_USER, configurationProvider.GetConfigurationValue(Constants.SAND_BOX_USER));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_PASSWORD, configurationProvider.GetConfigurationValue(Constants.SANDBOX_PASSWORD));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER, configurationProvider.GetConfigurationValue(Constants.SANDBOX_ACCOUNT_NUMBER));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_ID, configurationProvider.GetConfigurationValue(Constants.SANDBOX_CUSTOMER_ID));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_FIRST_NAME, configurationProvider.GetConfigurationValue(Constants.SANDBOX_CUSTOMER_FIRST_NAME));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_LAST_NAME, configurationProvider.GetConfigurationValue(Constants.SANDBOX_CUSTOMER_LAST_NAME));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_EMAIL, configurationProvider.GetConfigurationValue(Constants.SANDBOX_CUSTOMER_EMAIL));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_MOBILE, configurationProvider.GetConfigurationValue(Constants.SANDBOX_CUSTOMER_MOBILE));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_API_URL, configurationProvider.GetConfigurationValue(Constants.SANDBOX_URL));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_STREAMING_ACCOUNT_WEBSOCKET_URL, configurationProvider.GetConfigurationValue(Constants.SANDBOX_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL));
                    break;

                case Constants.PRODUCTION_RUN_MODE:
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_USER, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_USER));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_PASSWORD, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_PASSWORD));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_ACCOUNT_NUMBER, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_ACCOUNT_NUMBER));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_ID, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_CUSTOMER_ID));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_FIRST_NAME, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_CUSTOMER_FIRST_NAME));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_LAST_NAME, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_CUSTOMER_LAST_NAME));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_EMAIL, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_CUSTOMER_EMAIL));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_CUSTOMER_MOBILE, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_CUSTOMER_MOBILE));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_API_URL, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_URL));
                    configurationProvider.SetConfigurationValue(Constants.ACTIVE_STREAMING_ACCOUNT_WEBSOCKET_URL, configurationProvider.GetConfigurationValue(Constants.PRODUCTION_STREAMING_ACCOUNT_DATA_WEBSOCKET_URL));
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

            configurationProvider.SetConfigurationValue(Constants.ALPHA_VANTAGE_API_KEY, "A5069SA46CZPGTHQ");
            #endregion


        }
    }
}
