using System;
using System.Reflection;
using System.Text.Json;

namespace TangoBot.App.DTOs
{
    public abstract class AbstractDTO
    {
        protected AbstractDTO(JsonDocument jsonDocument)
        {
            if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataElement))
            {
                if (dataElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in dataElement.EnumerateArray())
                    {
                        PopulateProperties(item);
                    }
                }
                else
                {
                    PopulateProperties(dataElement);
                }
            }
        }

        private void PopulateProperties(JsonElement element)
        {
            foreach (var property in element.EnumerateObject())
            {
                var camelCaseName = ConvertToCamelCase(property.Name);
                var propertyInfo = GetType().GetProperty(camelCaseName, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    var value = JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType);
                    propertyInfo.SetValue(this, value);
                }
                else
                {
                    Console.WriteLine($"Property {camelCaseName} not found or not writable.");
                }
            }
        }

        private string ConvertToCamelCase(string hyphenSeparatedName)
        {
            var parts = hyphenSeparatedName.Split('-');
            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
            }
            return string.Join(string.Empty, parts);
        }
    }
}
