using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace TangoBot.App.DTOs
{
    public abstract class AbstractDTO
    {
        protected AbstractDTO(JsonDocument jsonDocument)
        {
            
            // Check if the document has a "data" property and if it is an array
            // bool documentHasData = jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataElement);
            //bool isDataAnArray = dataElement.TryGetProperty("items", out JsonElement itemsElement) && itemsElement.ValueKind == JsonValueKind.Array;

            if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataElement))
            {
                if (dataElement.TryGetProperty("items", out JsonElement itemsElement) && itemsElement.ValueKind == JsonValueKind.Array)
                {
                    var items = new List<AccountSnapShot>();
                    foreach (var item in itemsElement.EnumerateArray())
                    {
                        var accountSnapShot = new AccountSnapShot();
                        PopulateProperties(item, accountSnapShot);
                        items.Add(accountSnapShot);
                    }
                    var propertyInfo = GetType().GetProperty("Items", BindingFlags.Public | BindingFlags.Instance);
                    propertyInfo?.SetValue(this, items);
                }
            }
        }

        private void PopulateProperties(JsonElement element, object target)
        {
            foreach (var property in element.EnumerateObject())
            {
                var camelCaseName = ConvertToCamelCase(property.Name);
                var propertyInfo = target.GetType().GetProperty(camelCaseName, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    var value = ConvertJsonValue(property.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(target, value);
                }
                else
                {
                    Console.WriteLine($"Property {camelCaseName} not found or not writable.");
                }
            }
        }

        private object ConvertJsonValue(JsonElement jsonElement, Type targetType)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                var stringValue = jsonElement.GetString();
                if (targetType == typeof(double) && double.TryParse(stringValue, out var doubleValue))
                {
                    return doubleValue;
                }
                if (targetType == typeof(int) && int.TryParse(stringValue, out var intValue))
                {
                    return intValue;
                }
                if (targetType == typeof(decimal) && decimal.TryParse(stringValue, out var decimalValue))
                {
                    return decimalValue;
                }
                if (targetType == typeof(float) && float.TryParse(stringValue, out var floatValue))
                {
                    return floatValue;
                }
                if (targetType == typeof(long) && long.TryParse(stringValue, out var longValue))
                {
                    return longValue;
                }
            }

            return JsonSerializer.Deserialize(jsonElement.GetRawText(), targetType);
        }

        private string ConvertToCamelCase(string hyphenSeparatedName)
        {
            var parts = hyphenSeparatedName.Split('-');
            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
            }

            var camelCasedName = string.Join(string.Empty, parts);

            // Make first letter uppercase
            if (camelCasedName.Length > 0)
            {
                camelCasedName = char.ToUpper(camelCasedName[0]) + camelCasedName.Substring(1);
            }

            return camelCasedName;
        }


    }
}
