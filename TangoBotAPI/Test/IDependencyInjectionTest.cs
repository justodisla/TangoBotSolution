using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Test
{
    public interface IDependencyInjectionTest : IInfrService
    {
        /// <summary>
        /// Gets a value by key.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key.</returns>
        string GetValue(string key);

        /// <summary>
        /// Sets a value by key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        void SetValue(string key, string value);

        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>A dictionary containing all key-value pairs.</returns>
        IDictionary<string, string> GetAllValues();

        /// <summary>
        /// Saves the current values asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveValuesAsync();

        /// <summary>
        /// Deletes a value by key.
        /// </summary>
        /// <param name="key">The key of the value to delete.</param>
        void DeleteValue(string key);
    }
}
