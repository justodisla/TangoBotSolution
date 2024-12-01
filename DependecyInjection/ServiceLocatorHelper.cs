using System;
using System.Linq;
using System.Reflection;
using TangoBot.API.Configuration;
using TangoBot.API.Toolkit;

namespace TangoBot.DependecyInjection
{
    internal class ServiceLocatorHelper
    {
        private static bool _initialized;
        private static IConfigurationProvider? _configurationProvider;
        private static string _clearedNamespaces = string.Empty;

        internal static bool NamespaceCleared(Type t)
        {

            return true;

            if (!_initialized)
            {
                Initialize();
            }

            if (_clearedNamespaces != null)
            {
                var namespacesArray = _clearedNamespaces.Split(',');
                foreach (var ns in namespacesArray)
                {
                    if (IsNamespaceCleared(t.Namespace, ns.Trim()))
                    {
                        return true;
                    }
                }
            }

            //throw new Exception($"Namespace {t.Namespace} is not cleared for {t.FullName}");

            return false;
        }

        private static bool IsNamespaceCleared(string? targetNamespace, string clearedNamespace)
        {
            if (string.IsNullOrEmpty(targetNamespace) || string.IsNullOrEmpty(clearedNamespace))
            {
                return false;
            }

            var clearedSegments = clearedNamespace.Split('.');
            var targetSegments = targetNamespace.Split('.');

            int i = 0, j = 0;
            while (i < clearedSegments.Length && j < targetSegments.Length)
            {
                if (clearedSegments[i] == "**")
                {
                    return true;
                }

                if (clearedSegments[i] == "*")
                {
                    i++;
                    j++;
                    continue;
                }

                if (!clearedSegments[i].Equals(targetSegments[j], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                i++;
                j++;
            }

            // If we have processed all cleared segments and there are no remaining target segments, it's a match
            return i == clearedSegments.Length;
        }

        private static void Initialize()
        {
            _clearedNamespaces = "TangoBot.**";
            _initialized = true;
        }

        internal static bool ResolveAssignable(Type interfaceType, Type type)
        {
            Assembly assembly = type.Assembly;
            if (assembly.FullName.ToLower().Contains("logg"))
            {
                Console.WriteLine("Assembly: " + assembly.FullName);
            }

            if (interfaceType.IsGenericType && type.IsGenericType)
            {
                // Get the generic type definitions
                var interfaceGenericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                var typeGenericTypeDefinition = type.GetGenericTypeDefinition();

                // Check if the generic type definitions are the same
                if (interfaceGenericTypeDefinition == typeGenericTypeDefinition)
                {
                    return true;
                }

                // Check if the type implements the interface
                var implementedInterfaces = type.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType)
                    {
                        var implementedInterfaceGenericTypeDefinition = implementedInterface.GetGenericTypeDefinition();

                        // Check if the generic type definitions are the same
                        if (interfaceGenericTypeDefinition == implementedInterfaceGenericTypeDefinition)
                        {
                            return true;
                        }
                    }
                }
            }

            // Fallback to the default IsAssignableFrom check
            return interfaceType.IsAssignableFrom(type);
        }

        internal static bool xResolveAssignable(Type interfaceType, Type type)
        {

            //if(type.FullName.Contains("logg") && interfaceType.FullName.Contains("logg"))
            //{
            //    Console.WriteLine("Type: " + type.FullName);
            //}

            string interfaceName = interfaceType.Name ?? string.Empty;
            string typeName = type.Name ?? string.Empty;
            bool isAssignable = interfaceType.IsAssignableFrom(type);
            Assembly assembly = type.Assembly;

            if(assembly.FullName.ToLower().Contains("logg"))
            {
                Console.WriteLine("Assembly: " + assembly.FullName);
            }

            if (isAssignable)
            {
                Console.WriteLine($"Interface: {interfaceName}, Type: {typeName}, IsAssignable: {isAssignable}");
            }
            else
            {
                Console.WriteLine($"Interface: {interfaceName}, Type: {typeName}, IsAssignable: {isAssignable}");
            }

            if(type.FullName.ToLower().Contains("LoggingServic") && interfaceType.FullName.ToLower().Contains("logg"))
            {
                Console.WriteLine("Type: " + type.FullName);
            }

            return isAssignable;
        }

        internal static bool IsSolutionAssembly(Assembly assembly)
        {

            if (!IsAssembly(assembly.Location))
            {
                Console.WriteLine("Not an assembly: " + assembly.Location);
                return false;
            }
         
            if (assembly.FullName.Contains("TangoBot"))
            {
                Console.WriteLine("Assembly: " + assembly.FullName);
                return true;
            }

            // Define your criteria for solution assemblies here.
            // For example, you might check if the assembly name starts with "TangoBot".
            //return assembly.GetName().Name?.StartsWith("TangoBot") ?? false;
            return false;
        }

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
    }
}
