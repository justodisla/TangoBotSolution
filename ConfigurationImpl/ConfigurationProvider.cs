using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBotApi.Services.Configuration;

namespace TangoBot.Infrastructure.ConfigurationImpl
{
    /// <summary>
    /// Implements the <see cref="IConfigurationProvider"/> interface to provide configuration functionalities.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, string> _configuration;

        public ConfigurationProvider()
        {
            _configuration = LoadConfiguration("appsettings.json");
        }

        private Dictionary<string, string> LoadConfiguration(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        public string GetConfigurationValue(string key)
        {
            return _configuration.TryGetValue(key, out var value) ? value : string.Empty;
        }

        public IDictionary<string, string> GetAllConfigurationValues()
        {
            return new Dictionary<string, string>(_configuration);
        }

        public void SetConfigurationValue(string key, string value)
        {
            _configuration[key] = value;
            SaveConfigurationAsync().Wait();


        }

        public async Task SaveConfigurationAsync()
        {
            var json = JsonSerializer.Serialize(_configuration, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("appsettings.json", json);
        }

        public void ResetConfiguration()
        {
            ConfigurationHelper.PrintConfigurationFileContent("appsettings.json");

            _configuration.Clear();
            SaveConfigurationAsync().Wait();
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }

    public static class ConfigurationHelper
    {
        /// <summary>
        /// Prints the content of the configuration file to the console.
        /// </summary>
        /// <param name="filePath">The path to the configuration file.</param>
        public static void PrintConfigurationFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Configuration file '{filePath}' does not exist.");
                return;
            }

            var json = File.ReadAllText(filePath);
            var jsonDocument = JsonDocument.Parse(json);
            var formattedJson = JsonSerializer.Serialize(jsonDocument, new JsonSerializerOptions { WriteIndented = true });

            Console.WriteLine(formattedJson);
        }
    }

}
