using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TangoBot.Infrastructure.DependencyInjection;
using TangoBotApi.DI;

namespace TangoBotApi.Infrastructure
{
    /// <summary>
    /// Provides a service locator for resolving dependencies at runtime.
    /// </summary>
    public static class ServiceLocator
    {
        // The service provider that holds the registered services.
        private static IServiceProvider? _serviceProvider;

        // A dictionary to keep track of service implementations.
        private static readonly Dictionary<Type, List<Type>> _serviceImplementations = new();

        // A flag to indicate whether the service locator has been initialized.
        private static bool _initialized = false;

        // An object to lock on for thread safety.
        private static readonly object _lock = new();

        /// <summary>
        /// Initializes the service locator by scanning for DLLs and registering services.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            lock (_lock)
            {
                if (_initialized)
                {
                    return;
                }

                var baseDirectory = ServiceLocatorHelper.GetSearchDirectory();

                var serviceCollection = new ServiceCollection();

                // Scan for DLLs and load types that implement IInfrService
                // var dllFiles = Directory.GetFiles(baseDirectory, "*.dll", SearchOption.AllDirectories)
                //   .Where(dll => Path.GetFileName(dll).StartsWith("TangoBot.", StringComparison.OrdinalIgnoreCase));

                var dllFiles = Directory.GetFiles(baseDirectory, "*.dll", SearchOption.AllDirectories)
                                       .Where(dll => Path.GetFileName(dll).StartsWith("TangoBot.", StringComparison.OrdinalIgnoreCase) &&
                                       dll.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar));


                foreach (var dll in dllFiles)
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dll);
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName.FullName);

                    Console.WriteLine($"Loading assembly {assemblyName.FullName} from {dll}");

                    if (assembly == null)
                    {
                        assembly = Assembly.LoadFrom(dll);
                    }

                    var types = assembly.GetTypes().Where(t => typeof(IInfrService).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
                    foreach (var type in types)
                    {
                        var interfaces = type.GetInterfaces().Where(i => typeof(IInfrService).IsAssignableFrom(i) && !i.Name.Equals(typeof(IInfrService).Name));
                        foreach (var iface in interfaces)
                        {
                            if (!_serviceImplementations.ContainsKey(iface))
                            {
                                _serviceImplementations[iface] = new List<Type>();
                            }
                            _serviceImplementations[iface].Add(type);
                            serviceCollection.AddSingleton(iface, type); // Register as singleton by default
                        }
                    }
                }

                _serviceProvider = serviceCollection.BuildServiceProvider();
                _initialized = true;
            }
        }

        /// <summary>
        /// Gets a singleton service of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="qualifiedName">The qualified name of the implementation type (optional).</param>
        /// <returns>The singleton service instance.</returns>
        public static T GetSingletonService<T>(string? qualifiedName = null) where T : class
        {
            Initialize();

            if (qualifiedName == null)
            {
                return _serviceProvider!.GetRequiredService<T>();
            }

            var implementationType = GetImplementationType<T>(qualifiedName);
            return (T)_serviceProvider!.GetRequiredService(implementationType);
        }

        /// <summary>
        /// Gets a transient service of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="qualifiedName">The qualified name of the implementation type (optional).</param>
        /// <returns>The transient service instance.</returns>
        public static T GetTransientService<T>(string? qualifiedName = null) where T : class
        {
            Initialize();

            if (qualifiedName == null)
            {
                return _serviceProvider!.GetRequiredService<T>();
            }

            var implementationType = GetImplementationType<T>(qualifiedName);
            return (T)_serviceProvider!.GetService(implementationType)!;
        }

        /// <summary>
        /// Gets the implementation type for the specified interface and qualified name.
        /// </summary>
        /// <typeparam name="T">The type of the interface.</typeparam>
        /// <param name="qualifiedName">The qualified name of the implementation type.</param>
        /// <returns>The implementation type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no implementations are found for the interface or the qualified name does not match any implementation.</exception>
        private static Type GetImplementationType<T>(string qualifiedName) where T : class
        {
            var interfaceType = typeof(T);
            if (!_serviceImplementations.ContainsKey(interfaceType))
            {
                throw new InvalidOperationException($"No implementations found for {interfaceType.FullName}");
            }

            var implementationType = _serviceImplementations[interfaceType].FirstOrDefault(t => t.FullName == qualifiedName);
            if (implementationType == null)
            {
                throw new InvalidOperationException($"No implementation found for {interfaceType.FullName} with qualified name {qualifiedName}");
            }

            return implementationType;
        }
    }
}
