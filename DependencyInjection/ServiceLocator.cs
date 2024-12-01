using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TangoBotApi.DI;

namespace TangoBotApi.Infrastructure
{
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;
        private static readonly Dictionary<Type, List<Type>> _serviceImplementations = new();

        public static void Initialize(string baseDirectory)
        {
            var serviceCollection = new ServiceCollection();

            // Scan for DLLs and load types that implement IInfrService
            var dllFiles = Directory.GetFiles(baseDirectory, "*.dll", SearchOption.AllDirectories);
            foreach (var dll in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dll);
                var types = assembly.GetTypes().Where(t => typeof(IInfrService).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces().Where(i => typeof(IInfrService).IsAssignableFrom(i));
                    foreach (var iface in interfaces)
                    {
                        if (!_serviceImplementations.ContainsKey(iface))
                        {
                            _serviceImplementations[iface] = new List<Type>();
                        }
                        _serviceImplementations[iface].Add(type);
                        serviceCollection.AddTransient(iface, type); // Register as transient by default
                    }
                }
            }

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetSingletonService<T>(string qualifiedName = null) where T : class
        {
            if (qualifiedName == null)
            {
                return _serviceProvider.GetRequiredService<T>();
            }

            var implementationType = GetImplementationType<T>(qualifiedName);
            return (T)_serviceProvider.GetRequiredService(implementationType);
        }

        public static T GetTransientService<T>(string qualifiedName = null) where T : class
        {
            if (qualifiedName == null)
            {
                return _serviceProvider.GetRequiredService<T>();
            }

            var implementationType = GetImplementationType<T>(qualifiedName);
            return (T)_serviceProvider.GetService(implementationType);
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


