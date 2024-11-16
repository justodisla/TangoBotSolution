using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TangoBot.HttpClientLib
{
    public static class AccountInfoDeserializer
    {
        public static bool DeserializeAccountBalances(string responseBody, ref Dictionary<string, object> accountBalances, ref string context)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement))
                {
                    accountBalances = new Dictionary<string, object>();

                    foreach (var property in dataElement.EnumerateObject())
                    {
                        accountBalances[property.Name] = property.Value.GetString();
                    }
                }

                if (root.TryGetProperty("context", out JsonElement contextElement))
                {
                    context = contextElement.GetString();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return false;
            }
        }
    }
}
