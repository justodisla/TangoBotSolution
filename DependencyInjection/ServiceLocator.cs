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
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;
        private static readonly Dictionary<Type, List<Type>> _serviceImplementations = new();
        private static readonly Dictionary<Type, List<Type>> _delayedServices = new();
        private static bool _initialized = false;
        private static readonly object _lock = new();
        private static readonly HashSet<Type> _processedServices = new();

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

                var dllFiles = Directory.GetFiles(baseDirectory, "*.dll", SearchOption.AllDirectories)
                                       .Where(dll => Path.GetFileName(dll).StartsWith("TangoBot.", StringComparison.OrdinalIgnoreCase) &&
                                       dll.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar));

                foreach (var dll in dllFiles)
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dll);
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName.FullName);

                    if (assembly == null)
                    {
                        Console.WriteLine($"Loading assembly {assemblyName.FullName} from {dll}");
                        assembly = Assembly.LoadFrom(dll);
                    }
                }

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Console.WriteLine($"Checking assembly {assembly.GetName()}");

                    try
                    {
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

                                // Check if the type has any constructors that require parameters
                                var constructors = type.GetConstructors();
                                if (constructors.Any(c => c.GetParameters().Length > 0))
                                {
                                    continue;
                                }

                                if (_processedServices.Contains(type))
                                {
                                    continue;
                                }

                                var instance = (IInfrService)Activator.CreateInstance(type)!;
                                string[] requiredServices;
                                try
                                {
                                    requiredServices = instance.Requires();
                                }
                                catch (NotImplementedException)
                                {
                                    requiredServices = Array.Empty<string>();
                                }

                                if (requiredServices.All(rs =>
                                {
                                    var serviceType = Type.GetType(rs);
                                    return serviceType != null && _serviceImplementations.ContainsKey(serviceType);
                                }))
                                {
                                    serviceCollection.AddSingleton(iface, type);
                                    _processedServices.Add(type);
                                }
                                else
                                {
                                    if (!_delayedServices.ContainsKey(iface))
                                    {
                                        _delayedServices[iface] = new List<Type>();
                                    }
                                    _delayedServices[iface].Add(type);
                                }
                            }
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        foreach (var loaderException in ex.LoaderExceptions)
                        {
                            Console.WriteLine($"Error loading type from assembly {assembly.GetName()}: {loaderException.Message}");
                        }
                    }
                }

                _serviceProvider = serviceCollection.BuildServiceProvider();
                _initialized = true;

                RegisterDelayedServices(serviceCollection);
            }
        }


        private static void RegisterDelayedServices(ServiceCollection serviceCollection)
        {
            foreach (var delayedService in _delayedServices)
            {
                foreach (var type in delayedService.Value)
                {
                    // Check if the type has any constructors that require parameters
                    var constructors = type.GetConstructors();
                    if (constructors.Any(c => c.GetParameters().Length > 0))
                    {
                        continue;
                    }

                    if (_processedServices.Contains(type))
                    {
                        continue;
                    }

                    var instance = (IInfrService)Activator.CreateInstance(type)!;
                    string[] requiredServices;
                    try
                    {
                        requiredServices = instance.Requires();
                    }
                    catch (NotImplementedException)
                    {
                        requiredServices = Array.Empty<string>();
                    }

                    if (requiredServices.All(rs =>
                    {
                        var serviceType = Type.GetType(rs);
                        return serviceType != null && _serviceImplementations.ContainsKey(serviceType);
                    }))
                    {
                        serviceCollection.AddSingleton(delayedService.Key, type);
                        _processedServices.Add(type);
                    }
                }
            }

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

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
