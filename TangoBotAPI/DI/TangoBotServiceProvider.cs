using Microsoft.Extensions.DependencyInjection;
using System;

namespace TangoBotAPI.DI
{
    public static class TangoBotServiceProvider
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize(Action<IServiceCollection> configureServices)
        {
            var services = new ServiceCollection();
            configureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        public static T? GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public static T? GetRequiredService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
