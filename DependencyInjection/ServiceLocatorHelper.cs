using System;
using System.IO;
using System.Reflection;

namespace TangoBot.Infrastructure.DependencyInjection
{
    internal class ServiceLocatorHelper
    {
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
