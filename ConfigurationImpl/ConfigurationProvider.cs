using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TangoBotApi.Configuration;
using Microsoft.Extensions.Configuration;

namespace ConfigurationImpl
{
    /// <summary>
    /// Implements the <see cref="IConfigurationProvider"/> interface to provide configuration functionalities.
    /// </summary>
    public class ConfigurationProvider : TangoBotApi.Configuration.IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationProvider()
        {
            // Build the configuration from appsettings.json and environment variables
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public string GetConfigurationValue(string key)
        {
            return _configuration[key];
        }

        public IDictionary<string, string> GetAllConfigurationValues()
        {
            var configValues = new Dictionary<string, string>();
            foreach (var kvp in _configuration.AsEnumerable())
            {
                configValues[kvp.Key] = kvp.Value;
            }
            return configValues;
        }

        public void SetConfigurationValue(string key, string value)
        {
            // Note: IConfiguration is generally read-only. To set values, you might need a custom implementation or use a writable configuration source.
            // This is a placeholder implementation.
            throw new NotImplementedException("Setting configuration values is not supported in this implementation.");
        }

        public Task SaveConfigurationAsync()
        {
            // Note: Saving configuration is not typically supported with IConfiguration. This is a placeholder implementation.
            throw new NotImplementedException("Saving configuration is not supported in this implementation.");
        }
    }
}

