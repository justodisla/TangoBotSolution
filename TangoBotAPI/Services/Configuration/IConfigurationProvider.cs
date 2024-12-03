using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Configuration
{
    public interface IConfigurationProvider : IInfrService
    {
        /// <summary>
        /// Gets a configuration value by key.
        /// </summary>
        /// <param name="key">The key of the configuration value.</param>
        /// <returns>The configuration value.</returns>
        string GetConfigurationValue(string key);

        /// <summary>
        /// Gets all configuration values.
        /// </summary>
        /// <returns>A dictionary containing all configuration values.</returns>
        IDictionary<string, string> GetAllConfigurationValues();

        /// <summary>
        /// Sets a configuration value by key.
        /// </summary>
        /// <param name="key">The key of the configuration value.</param>
        /// <param name="value">The value to set.</param>
        void SetConfigurationValue(string key, string value);

        /// <summary>
        /// Saves the current configuration values.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveConfigurationAsync();

        /// <summary>
        /// Resets all configuration values.
        /// </summary>
        void ResetConfiguration();
    }
}
