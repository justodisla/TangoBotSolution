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

            //if(type.FullName.Contains("logg") && interfaceType.FullName.Contains("logg"))
            //{
            //    Console.WriteLine("Type: " + type.FullName);
            //}
            return interfaceType.IsAssignableFrom(type);
        }

        internal static bool IsSolutionAssembly(Assembly assembly)
        {
            // Define your criteria for solution assemblies here.
            // For example, you might check if the assembly name starts with "TangoBot".
            //return assembly.GetName().Name?.StartsWith("TangoBot") ?? false;
            return true;
        }
    }
}
