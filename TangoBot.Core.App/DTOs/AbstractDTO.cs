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
                foreach (var property in dataElement.EnumerateObject())
                {
                    var camelCaseName = ConvertToCamelCase(property.Name);
                    var propertyInfo = GetType().GetProperty(camelCaseName, BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        try
                        {
                            var value = JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType);
                            propertyInfo.SetValue(this, value);
                        }
                        catch (JsonException)
                        {
                            if (propertyInfo.PropertyType == typeof(double) && double.TryParse(property.Value.GetString(), out double doubleValue))
                            {
                                propertyInfo.SetValue(this, doubleValue);
                            }
                            else
                            {
                                Console.WriteLine($"Failed to deserialize property {camelCaseName}.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Property {camelCaseName} not found or not writable.");
                    }
                }
            }
        }

        private string ConvertToCamelCase(string hyphenSeparatedName)
        {
            var parts = hyphenSeparatedName.Split('-');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
            }
            return string.Join(string.Empty, parts);
        }
    }
}
