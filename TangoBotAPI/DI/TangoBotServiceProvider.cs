using Microsoft.Extensions.DependencyInjection;
using System;

namespace TangoBotAPI.DI
{
    public static class TangoBotServiceProvider
    {
        private static IServiceProvider? _wrappedServiceProvider;
        private static bool initialize = false;
        private static ServiceCollection? services;

        public static void Initialize()
        {
            if (initialize)
            {
                return;
            }

            services = new ServiceCollection();
            _wrappedServiceProvider = services.BuildServiceProvider() ?? throw new Exception("ServiceProvider build failed");

            initialize = true;
        }

        public static void AddSingletonService<T>(T service) where T : class
        {
            if (!initialize)
            {
                Initialize();
            }
            (services ?? throw new Exception("Services is null")).AddSingleton(service);
            _wrappedServiceProvider = services?.BuildServiceProvider();
        }

        public static T? GetService<T>()
        {
            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

            return _wrappedServiceProvider.GetService<T>();// ?? throw new Exception("Unable to return service " + typeof(T));
        }

        public static T? GetRequiredService<T>()
        {

            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            return _wrappedServiceProvider.GetRequiredService<T>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        }
    }
}
