using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBotApi.Configuration;

namespace ConfigurationImpl
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
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public string GetConfigurationValue(string key)
        {
            return _configuration.TryGetValue(key, out var value) ? value : null;
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
    }
}
