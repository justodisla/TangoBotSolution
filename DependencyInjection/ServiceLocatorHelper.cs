using System;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace TangoBot.Infrastructure.DependencyInjection
{
    internal class ServiceLocatorHelper
    {
        internal static void CheckForParameterizedConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Any(c => c.GetParameters().Length > 0))
            {
                throw new Exception($"Type {type.Name} has a parameterized constructor.");
            }
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

        internal static void VerifyItIsLegalQualifiedName(string? qualifiedName)
        {
            if(string.IsNullOrEmpty(qualifiedName))
            {
                return;
            }

            var parts = qualifiedName.Split('.');
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part) || !char.IsLetter(part[0]) || part.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
                {
                    throw new ArgumentException($"Invalid qualified name: {qualifiedName}", nameof(qualifiedName));
                }
            }
        }
    }
}
