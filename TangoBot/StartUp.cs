using HttpClientLib.AccountApi;
using HttpClientLib.TokenManagement;
using Microsoft.Extensions.DependencyInjection;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using TangoBotServiceProvider = TangoBotAPI.DI.TangoBotServiceProvider;

namespace TangoBot
{
    internal class StartUp
    {
        internal static void InitializeDI()
        {
            TangoBotServiceProvider.Initialize(services =>
            {
                // Register HttpClient
                //services.AddHttpClient();

                services.AddSingleton<HttpClient>();

                // Register TokenProvider
                services.AddSingleton<TokenProvider>();

                // Register AccountComponent
                services.AddTransient<AccountComponent>();
            });

            // Resolve the AccountComponent to use it
            var accountComponent = TangoBotServiceProvider.GetService<AccountComponent>();

           var tp = TangoBotServiceProvider.GetService<TokenProvider>();

            var vt = tp.GetValidTokenAsync();
        }
    }
}
