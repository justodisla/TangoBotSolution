using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TangoBotAPI.DI
{
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

        /// <summary>
        /// Adds a singleton service to the service collection.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="service">The service instance.</param>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        public static void AddSingletonService<T>(T service, string name = "") where T : class
        {
            if (!initialize)
            {
                Initialize();
            }

            if (!string.IsNullOrEmpty(name))
            {
                namedServices[name] = typeof(T);
            }

            (services ?? throw new Exception("Services is null")).AddSingleton(service);
            _wrappedServiceProvider = services?.BuildServiceProvider();
        }

        /// <summary>
        /// Gets a service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? GetService<T>(string name = "")
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

        /// <summary>
        /// Gets a required service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">An optional name for the service to support multiple implementations.</param>
        /// <returns>The service instance.</returns>
        public static T? GetRequiredService<T>(string name = "")
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

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            return _wrappedServiceProvider.GetRequiredService<T>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        }
    }
}
