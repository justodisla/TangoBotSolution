using Microsoft.Extensions.DependencyInjection;
using System;

namespace TangoBotAPI.DI
{
    public static class TangoBotServiceProvider
    {
        private static IServiceProvider? _serviceProvider;
        private static bool initialize = false;
        private static ServiceCollection? services;

        public static void Initialize()
        {
            if (initialize)
            {
                return;
            }

                services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();

            initialize = true;
        }

        public static void AddSingletonService<T>(T service) where T : class
        {
            if (!initialize)
            {
                Initialize();
            }
            services?.AddSingleton(service);
            _serviceProvider = services.BuildServiceProvider();
        }

        public static T? GetService<T>()
        {
            if(!initialize)
            {
                Initialize();
            }
            return _serviceProvider.GetService<T>();
        }

        public static T? GetRequiredService<T>()
        {
            if (!initialize)
            {
                Initialize();
            }
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
