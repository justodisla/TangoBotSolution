using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.API.Configuration;

namespace TangoBotAPI.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly string _filePath;
        private IDictionary<string, string> _configurations;
        private bool IsInitialized;

        public ConfigurationProvider()
        {
            if(IsInitialized)
            {
                return;
            }
            _filePath = "TBConfig/config.json";
            _configurations = new Dictionary<string, string>();

            //ResetConfigurationAsync().Wait();

            LoadConfiguration();

            IsInitialized = true;
        }

        public string? GetConfigurationValue(string key)
        {
            return _configurations.TryGetValue(key, out var value) ? value : null;
        }

        public IDictionary<string, string> GetAllConfigurationValues()
        {
            return new Dictionary<string, string>(_configurations);
        }

        public void SetConfigurationValue(string key, string value)
        {
            if (_configurations.TryGetValue(key, out var existingValue) && existingValue == value)
            {
                return;
            }
            _configurations[key] = value;
            SaveConfigurationAsync().Wait();
        }

        public async Task SaveConfigurationAsync()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            }
            var json = JsonSerializer.Serialize(_configurations, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        private void LoadConfiguration()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _configurations = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
        }

        public async Task ResetConfigurationAsync()
        {
            _configurations.Clear();
            await SaveConfigurationAsync();
        }
    }
}
