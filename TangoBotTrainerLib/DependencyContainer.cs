using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public static class DependencyInjection
    {
        private static readonly Dictionary<Type, object> _registeredComponents = new();
        private static bool _isInitialized = false;

        /// <summary>
        /// Scans the given path for DLLs and registers all types implementing ITbotComponent.
        /// </summary>
        private static void Initialize()
        {
            if (_isInitialized) return;

            var dllFiles = Directory.GetFiles(GetSearchDirectory(), "*.dll", SearchOption.AllDirectories);

            foreach (var dllPath in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(ITbotComponent).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                        {
                            var interfaces = type.GetInterfaces();
                            var instance = Activator.CreateInstance(type);
                            foreach (var i in interfaces)
                            {
                                if (i == typeof(ITbotComponent) || i == typeof(ICloneable))
                                {
                                    continue;
                                }
                                Register(i, instance);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading assembly from {dllPath}: {ex.Message}");
                }
            }

            _isInitialized = true;
        }

        /// <summary>
        /// Registers a type and its instance for dependency injection.
        /// </summary>
        private static void Register(Type type, object instance)
        {
            if (!_registeredComponents.ContainsKey(type))
            {
                _registeredComponents[type] = instance;
                Console.WriteLine($"Registered component: {type.FullName}");
            }
        }

        /// <summary>
        /// Resolves a component of the specified type.
        /// </summary>
        public static T? Resolve<T>() where T : class
        {
            if (!_isInitialized)
            {
                // Initialize with the default path or a configurable path
                Initialize(); // Default to application directory
            }

            var type = typeof(T);

            if (_registeredComponents.TryGetValue(type, out var instance))
            {
                return instance as T;
            }

            throw new InvalidOperationException($"No component registered for type {type.FullName}.");
        }

        internal static string GetSearchDirectory()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var directory = Directory.GetParent(currentAssembly.Location);

            while (directory != null && !Directory.GetFiles(directory.FullName, "*.sln").Any())
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                throw new Exception("Solution directory not found.");
            }

            return directory.FullName;
        }
    }
}