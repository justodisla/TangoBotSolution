using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TangoBotAPI.DI
{
    [Obsolete("This class is obsolete, use TangoBotServiceProviderExp instead")]
    /// <summary>
    /// Provides a service provider for dependency injection, allowing registration and resolution of services.
    /// Supports named services to register multiple implementations of the same interface.
    /// </summary>
    public static class TangoBotServiceProvider
    {
        private static IServiceProvider? _wrappedServiceProvider;
        private static bool initialize = false;
        private static ServiceCollection? services;
        private static readonly Dictionary<string, Type> namedServices = new();

        /// <summary>
        /// Initializes the service provider if it has not been initialized already.
        /// </summary>
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
        [Obsolete("This method is obsolete, use AddService<T> instead")]
        /// <summary>
        /// Adds a service to the service collection.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="implementationFactory">The factory to create the service instance.</param>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        public static void AddService<T>(Func<IServiceProvider, T> implementationFactory, string name = "") where T : class
        {
            if (!initialize)
            {
                Initialize();
            }

            if (!string.IsNullOrEmpty(name))
            {
                namedServices[name] = typeof(T);
            }

            (services ?? throw new Exception("Services is null")).AddSingleton(implementationFactory);
            _wrappedServiceProvider = services?.BuildServiceProvider();
        }
        [Obsolete("This method is obsolete, use AddService<T> instead")]
        /// <summary>
        /// Gets a singleton service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? xGetSingletonService<T>(string name = "")
        {
            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

            if (!string.IsNullOrEmpty(name) && namedServices.TryGetValue(name, out var serviceType))
            {
                return (T?)_wrappedServiceProvider.GetService(serviceType);
            }

            return _wrappedServiceProvider.GetService<T>();
        }
        [Obsolete("This method is obsolete, use GetSingletonService<T> instead")]
        /// <summary>
        /// Gets a transient service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? xGetTransientService<T>(string name = "")
        {
            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

            if (!string.IsNullOrEmpty(name) && namedServices.TryGetValue(name, out var serviceType))
            {
                return (T?)_wrappedServiceProvider.GetRequiredService(serviceType);
            }

            return _wrappedServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Gets a service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? xGetService<T>(string name = "")
        {
            return xGetSingletonService<T>(name);
        }
    }
}
