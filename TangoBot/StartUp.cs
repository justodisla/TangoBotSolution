using HttpClientLib.AccountApi;
using HttpClientLib.CustomerApi;
using HttpClientLib.InstrumentApi;
using HttpClientLib.OrderApi;
using HttpClientLib.TokenManagement;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Pipelines;
using System.Xml.Serialization;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Streaming;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using TangoBotStreaming.Services;
using TangoBotServiceProvider = TangoBotAPI.DI.TangoBotServiceProvider;

namespace TangoBot
{
    internal class StartUp
    {
        internal static void InitializeDI()
        {
            
            TangoBotServiceProvider.AddSingletonService<IConfigurationProvider>(new ConfigurationProvider());

            IConfigurationProvider? configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();
            
            configurationProvider
                .SetConfigurationValue(Constants.SANDBOX_URL, "https://api.cert.tastyworks.com");

            configurationProvider
                .SetConfigurationValue(Constants.PRODUCTION_URL, "https://api.tastyworks.com");

            configurationProvider
                .SetConfigurationValue("UserName", "tangobotsandboxuser");

            configurationProvider
                .SetConfigurationValue("Password", "TTTangoBotSandBoxPass");

            configurationProvider
                .SetConfigurationValue("apiQuoteTokenEndpoint", "/api-quote-tokens");

            configurationProvider
                .SetConfigurationValue("dxlink-url", "wss://tasty-openapi-ws.dxfeed.com/realtime");

            //--------

            TangoBotServiceProvider.AddSingletonService<HttpClient>(new HttpClient());

            TangoBotServiceProvider.AddSingletonService<ITokenProvider>(new TokenProvider());

            TangoBotServiceProvider.AddSingletonService<AccountComponent>(new AccountComponent());
            TangoBotServiceProvider.AddSingletonService<CustomerComponent>(new CustomerComponent());
            TangoBotServiceProvider.AddSingletonService<OrderComponent>(new OrderComponent());
            TangoBotServiceProvider.AddSingletonService<InstrumentComponent>(new InstrumentComponent());

            var vt = TangoBotServiceProvider.GetService<ITokenProvider>().GetValidStreamingToken().Result;

            //TangoBotServiceProvider.AddSingletonService<IStreamService<QuoteDataHistory>>(new StreamingService());

            // Resolve the AccountComponent to use it
            var accountComponent = TangoBotServiceProvider.GetService<AccountComponent>();

            var tp = TangoBotServiceProvider.GetService<ITokenProvider>();

            var vts = tp.GetValidTokenAsync();

            //Console.WriteLine("Token: " + vt.Result);
        }
    }
}
