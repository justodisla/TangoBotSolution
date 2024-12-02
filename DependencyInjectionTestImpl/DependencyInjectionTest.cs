using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Test;

namespace TangoBot.Infrastructure.DependencyInjectionTestImpl
{
    public class DependencyInjectionTest : IDependencyInjectionTest
    {
        private readonly Dictionary<string, string> _values;

        public DependencyInjectionTest()
        {
            _values = new Dictionary<string, string>();
        }

        public string GetValue(string key)
        {
            return _values.TryGetValue(key, out var value) ? value : string.Empty;
        }

        public void SetValue(string key, string value)
        {
            _values[key] = value;
        }

        public IDictionary<string, string> GetAllValues()
        {
            return new Dictionary<string, string>(_values);
        }

        public async Task SaveValuesAsync()
        {
            // Simulate an asynchronous save operation
            await Task.Delay(100);
        }

        public void DeleteValue(string key)
        {
            _values.Remove(key);
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }
    }
}
