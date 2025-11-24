using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TangoBot.API.Logging;

namespace TangoBot.DependecyInjection
{
    /// <summary>
    /// The `TangoBotServiceLocator` is a custom implementation of the Service Locator pattern designed to manage the lifecycle 
    /// and resolution of services in the TangoBot application. It dynamically discovers, registers, and resolves services 
    /// at runtime, supporting both singleton and transient lifecycles. This class is built on top of the 
    /// `Microsoft.Extensions.DependencyInjection` framework and provides additional functionality for dynamic service discovery 
    /// and logging.
    /// 
    /// <h3>Features:</h3>
    /// - **Dynamic Service Discovery**: Automatically discovers and registers services based on their interface and implementation.
    /// - **Singleton and Transient Lifecycles**: Supports both singleton and transient service lifecycles.
    /// - **Thread-Safe**: Ensures thread safety during service registration and resolution.
    /// - **Custom Logging**: Integrates with `ILoggingService` for structured logging, with a fallback to `Console.WriteLine`.
    /// - **Assembly Scanning**: Dynamically scans assemblies in the solution to locate service implementations.
    /// 
    /// <h3>Usage Instructions:</h3>
    /// 
    /// <h4>1. Initialization</h4>
    /// Before using the `TangoBotServiceLocator`, it must be initialized. This sets up the internal service collection 
    /// and resolves the logging service.
    /// 
    /// <code>
    /// TangoBotServiceLocator.Initialize();
    /// </code>
    /// 
    /// <h4>2. Resolving Singleton Services</h4>
    /// To resolve a singleton service, use the `GetSingletonService<T>()` method. If the service is not already registered, 
    /// it will be discovered, registered, and instantiated.
    /// 
    /// <code>
    /// var myService = TangoBotServiceLocator.GetSingletonService<IMyService>();
    /// </code>
    /// 
    /// Optionally, you can specify the fully qualified name of the implementation if multiple implementations exist:
    /// 
    /// <code>
    /// var myService = TangoBotServiceLocator.GetSingletonService<IMyService>("MyNamespace.MyServiceImplementation");
    /// </code>
    /// 
    /// <h4>3. Resolving Transient Services</h4>
    /// To resolve a transient service, use the `GetTransientService<T>()` method. A new instance of the service will be created 
    /// each time it is resolved.
    /// 
    /// <code>
    /// var myService = TangoBotServiceLocator.GetTransientService<IMyService>();
    /// </code>
    /// 
    /// <h4>4. Logging</h4>
    /// The `TangoBotServiceLocator` uses an `ILoggingService` for structured logging. If the logging service is unavailable, 
    /// it falls back to `Console.WriteLine`.
    /// 
    /// Example of logging:
    /// <code>
    /// TangoBotServiceLocator.LogInfo("Service initialized successfully", null);
    /// </code>
    /// 
    /// <h3>Implementation Details:</h3>
    /// - **Service Registration**: Services are registered dynamically when they are first resolved. The `DiscoverServiceType<T>()` 
    ///   method scans assemblies to locate the implementation of the requested interface.
    /// - **Thread Safety**: The `serviceCollectionLock` ensures that service registration and resolution are thread-safe.
    /// - **Caching**: Singleton instances are cached in the `singletonInstances` dictionary to avoid redundant instantiations.
    /// - **Error Handling**: Throws detailed exceptions if a service cannot be resolved or if multiple implementations are found 
    ///   without a specified name.
    /// 
    /// <h3>Limitations:</h3>
    /// - **Performance Overhead**: Rebuilding the `IServiceProvider` during runtime can be expensive.
    /// - **Service Locator Anti-Pattern**: While functional, the Service Locator pattern is generally discouraged in favor of 
    ///   dependency injection frameworks.
    /// - **Assembly Scanning**: Service discovery is limited to assemblies in the solution directory.
    /// 
    /// <h3>Recommendations:</h3>
    /// - Use `TangoBotServiceLocator` for dynamic service resolution in scenarios where traditional dependency injection is not feasible.
    /// - For new development, consider migrating to a standard dependency injection framework like `Microsoft.Extensions.DependencyInjection`.
    /// </summary>
    public class TangoBotServiceLocator
    {

        private static IServiceProvider? _wrappedServiceProvider;
        private static bool initialize = false;
        private static ServiceCollection? services;
        private static readonly ConcurrentDictionary<string, Type> namedServices = new();
        private static readonly ConcurrentDictionary<string, object> singletonInstances = new();
        private static readonly object serviceCollectionLock = new();
        private static ILoggingService _logger;

        

        /// <summary>
        /// Initializes the service provider if it has not been initialized already.
        /// </summary>
        public static void Initialize()
        {
            if (initialize)
            {
                LogInfo("Initializing ServiceLocator", null);
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

                LogInfo("Logging before _logger", null);

                _logger = GetSingletonService<ILoggingService>() ?? throw new Exception("Logger could not be loaded");

                LogInfo("ServiceLocator initialized", null);

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
                        if (serviceType.IsGenericTypeDefinition)
                        {
                            services?.AddSingleton(typeof(T), serviceType);
                        }
                        else
                        {
                            services?.AddSingleton(typeof(T), sp => Activator.CreateInstance(serviceType));
                        }
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
                 .Where(ServiceLocatorHelper.IsSolutionAssembly) // Filter assemblies using the new method
                 .ToList();

            var serviceTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && ServiceLocatorHelper.NamespaceCleared(t) && ServiceLocatorHelper.ResolveAssignable(interfaceType, t))
                .ToList();

            if (serviceTypes.Count == 0)
            {
                return null;
            }

            if (serviceTypes.Count > 1 && string.IsNullOrEmpty(name))
            {

                var implementations = serviceTypes.Select(t => $"Namespace: {t.GetType().Namespace}, Type: {t.GetType().Name}, DLL: {t.GetType().Assembly.Location}");
                var message = $"Multiple implementations found for interface '{interfaceType.Name}'. Please specify the implementation name. Available implementations:\n{string.Join("\n", implementations)}";
                throw new Exception(message);

                //throw new Exception($"Multiple implementations found for interface '{interfaceType.Name}'. Please specify the implementation name.");
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

        #region Logging
        private static void LogInfo(string message, object[] args)
        {
            Log("Info", message, args);
        }

        private static void LogError(Exception exception, string args)
        {
            Log("Error", null, null, exception, args);
        }

        private static void LogDebug(string message, object[] args)
        {
            Log("Debug", message, args);
        }

        private static void LogWarning(string message, object[] args)
        {
            Log("Warning", message, args);
        }

        private static void Log(string msType, string message, object[] args = null, Exception exception = null, string stArgs = null)
        {

            string className = typeof(TangoBotServiceLocator).Name;
            Type locatorType = typeof(TangoBotServiceLocator);


            switch (msType)
            {
                case "Info":
                    if (_logger == null)
                    {
                        Console.WriteLine($"[{className}] {message}");
                    }
                    else
                    {

                        _logger.LogInformation(locatorType, message, args);
                    }
                    break;
                case "Error":
                    if (_logger == null)
                    {
                        Console.WriteLine($"[{className}] {message}");
                    }
                    else
                    {
                        _logger.LogError(locatorType, exception, stArgs);
                    }
                    break;
                case "Debug":
                    if (_logger == null)
                    {
                        Console.WriteLine($"[{className}] {message}");
                    }
                    else
                    {
                        _logger.LogInformation(locatorType, message, args);
                    }
                    break;
                case "Warning":
                    if (_logger == null)
                    {
                        Console.WriteLine($"[{className}] {message}");
                    }
                    else
                    {
                        _logger.LogWarning(locatorType, message, args);
                    }
                    break;
                default:
                    if (_logger == null)
                    {
                        Console.WriteLine($"[{className}] {message}");
                    }
                    else
                    {
                        _logger.LogInformation(locatorType, message, args);
                    }
                    break;
            }

        }
        #endregion

    }
}
