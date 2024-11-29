using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TangoBot.DependecyInjection
{
    public static class TangoBotServiceLocator
    {
        private static IServiceProvider? _wrappedServiceProvider;
        private static bool initialize = false;
        private static ServiceCollection? services;
        private static readonly ConcurrentDictionary<string, Type> namedServices = new();
        private static readonly ConcurrentDictionary<string, object> singletonInstances = new();
        private static readonly object serviceCollectionLock = new();

        /// <summary>
        /// Initializes the service provider if it has not been initialized already.
        /// </summary>
        public static void Initialize()
        {
            if (initialize)
            {
                return;
            }

            lock (serviceCollectionLock)
            {
                if (initialize)
                {
                    return;
                }

                services = new ServiceCollection();
                _wrappedServiceProvider = services.BuildServiceProvider() ?? throw new Exception("ServiceProvider build failed");

                initialize = true;
            }
        }

        /// <summary>
        /// Gets a singleton service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="name">The fully qualified name of the service implementation class (optional).</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? GetSingletonService<T>(string name = "") where T : class
        {
            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

            var serviceType = DiscoverServiceType<T>(name) ?? throw new Exception($"Service implementation '{name}' not found for interface '{typeof(T).Name}'");

            name = LevelName(name, serviceType);

            if (!singletonInstances.ContainsKey(name))
            {
                lock (serviceCollectionLock)
                {

                    if (!namedServices.ContainsKey(name))
                    {
                        services?.AddSingleton(typeof(T), serviceType);
                        namedServices[name] = serviceType;
                        _wrappedServiceProvider = services?.BuildServiceProvider();
                    }

                    if (_wrappedServiceProvider == null)
                    {
                        throw new Exception("ServiceProvider build failed");
                    }

                    var instance = _wrappedServiceProvider.GetService<T>();
                    if (instance != null)
                    {
                        singletonInstances[name] = instance;
                    }
                }
            }

            return singletonInstances[name] as T;
        }

        private static string LevelName(string? name, Type serviceType)
        {
            name = string.IsNullOrEmpty(name) ? serviceType.FullName : name;

            // Reformat fullName to be used as a dictionary key
            name = Regex.Replace(name, @"[^a-zA-Z0-9_]", "_");
            return name;
        }

        /// <summary>
        /// Gets a transient service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="qualifiedName">The fully qualified name of the service implementation class (optional).</param>
        /// <returns>The service instance, or null if not found.</returns>
        public static T? GetTransientService<T>(string qualifiedName = "") where T : class
        {
            if (!initialize)
            {
                Initialize();
            }

            if (_wrappedServiceProvider == null)
            {
                throw new Exception("Service provider is null");
            }

            var serviceType = DiscoverServiceType<T>(qualifiedName);

            qualifiedName = LevelName(qualifiedName, serviceType);

            if (serviceType == null)
            {
                throw new Exception($"Service implementation '{qualifiedName}' not found for interface '{typeof(T).Name}'");
            }

            if (!namedServices.ContainsKey(qualifiedName))
            {
                lock (serviceCollectionLock)
                {
                    if (!namedServices.ContainsKey(qualifiedName))
                    {
                        services?.AddTransient(typeof(T), serviceType);
                        namedServices[qualifiedName] = serviceType;
                        _wrappedServiceProvider = services?.BuildServiceProvider();
                    }
                }
            }

            return _wrappedServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Discovers the service type that implements the specified interface and matches the given name.
        /// </summary>
        /// <typeparam name="T">The type of the service interface.</typeparam>
        /// <param name="name">The fully qualified name of the service implementation class (optional).</param>
        /// <returns>The service type, or null if not found.</returns>
        private static Type? DiscoverServiceType<T>(string name) where T : class
        {
            var interfaceType = typeof(T);
            var currentAssembly = Assembly.GetExecutingAssembly();
            var parentDirectory = (Directory.GetParent(currentAssembly.Location)?.Parent?.Parent?.FullName) ?? throw new Exception("Parent directory not found.");
            var assemblies = Directory.GetFiles(parentDirectory, "*.dll", SearchOption.AllDirectories)
                .Where(IsAssembly)
                .Select(Assembly.LoadFrom)
                .ToList();

            var serviceTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

            if (serviceTypes.Count == 0)
            {
                return null;
            }

            if (serviceTypes.Count > 1 && string.IsNullOrEmpty(name))
            {
                throw new Exception($"Multiple implementations found for interface '{interfaceType.Name}'. Please specify the implementation name.");
                /*
                throw new InvalidOperationException(
    $"Multiple implementations found for {typeof(T).FullName}. " +
    $"Please specify the full name of the desired implementation. Available implementations: " +
    $"{string.Join(", ", discoveredTypes.Select(t => t.FullName))}"
);*/
            }

            if (!string.IsNullOrEmpty(name) && !name.Contains('.'))
            {
                throw new Exception($"The name '{name}' is not a fully qualified type name.");
            }

            var serviceType = string.IsNullOrEmpty(name)
                ? serviceTypes.FirstOrDefault()
                : serviceTypes.FirstOrDefault(t => t.FullName.Equals(name, StringComparison.OrdinalIgnoreCase) || t.FullName.Split('`')[0].Equals(name, StringComparison.OrdinalIgnoreCase));

            if (serviceType == null)
            {
                throw new Exception($"Service implementation '{name}' not found for interface '{interfaceType.Name}'");
            }

            return serviceType;
        }

        /// <summary>
        /// Checks if the file is a valid .NET assembly.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>True if the file is a valid .NET assembly, otherwise false.</returns>
        private static bool IsAssembly(string filePath)
        {
            try
            {
                AssemblyName.GetAssemblyName(filePath);
                return true;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
            catch (FileLoadException)
            {
                return false;
            }
        }

        //A method that return true if a type belongs in any of a given collection of 
        //namespaces

    }
}
