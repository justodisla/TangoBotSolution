using HttpClientLib.AccountApi;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                        accountBalances[property.Name] = property.Value.GetString() ?? string.Empty;
                    }
                }

                if (root.TryGetProperty("context", out JsonElement contextElement))
                {
                    context = contextElement.GetString() ?? string.Empty;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return false;
            }
        }

        public static bool DeserializeBalanceSnapshots(string responseBody, ref Dictionary<string, object>[] balanceSnapshots, ref string context)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.TryGetProperty("items", out JsonElement itemsElement))
                {
                    var snapshotsList = new List<Dictionary<string, object>>();

                    foreach (var item in itemsElement.EnumerateArray())
                    {
                        var snapshot = new Dictionary<string, object>();

                        foreach (var property in item.EnumerateObject())
                        {
                            snapshot[property.Name] = property.Value.GetString() ?? string.Empty;
                        }

                        snapshotsList.Add(snapshot);
                    }

                    balanceSnapshots = snapshotsList.ToArray();
                }

                if (root.TryGetProperty("context", out JsonElement contextElement))
                {
                    context = contextElement.GetString() ?? string.Empty;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return false;
            }
        }

        public static bool DeserializeAccountPositions(string responseBody, ref Dictionary<string, object>[] accountPositions, ref string context)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.TryGetProperty("items", out JsonElement itemsElement))
                {
                    var positionsList = new List<Dictionary<string, object>>();

                    foreach (var item in itemsElement.EnumerateArray())
                    {
                        var position = new Dictionary<string, object>();

                        foreach (var property in item.EnumerateObject())
                        {
                            position[property.Name] = property.Value.GetString() ?? string.Empty;
                        }

                        positionsList.Add(position);
                    }

                    accountPositions = positionsList.ToArray();
                }

                if (root.TryGetProperty("context", out JsonElement contextElement))
                {
                    context = contextElement.GetString() ?? string.Empty;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return false;
            }
        }

        public static List<Account> DeserializeCustomerAccounts(string responseBody)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.TryGetProperty("items", out JsonElement itemsElement))
                {
                    var accountsList = new List<Account>();

                    foreach (var item in itemsElement.EnumerateArray())
                    {
                        if (item.TryGetProperty("account", out JsonElement accountElement))
                        {
                            var account = JsonSerializer.Deserialize<Account>(accountElement.GetRawText());
                            if (account != null)
                            {
                                accountsList.Add(account);
                            }
                        }
                    }

                    return accountsList;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return null;
            }
        }

        public static Account DeserializeAccount(string responseBody)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement))
                {
                    var account = JsonSerializer.Deserialize<Account>(dataElement.GetRawText());
                    return account;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return null;
            }
        }

        public class AccountData
        {
            [JsonPropertyName("data")]
            public AccountItems Data { get; set; } = new AccountItems();
        }

        public class AccountItems
        {
            [JsonPropertyName("items")]
            public List<AccountItem> Items { get; set; } = new List<AccountItem>();
        }

        public class AccountItem
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("balance")]
            public decimal Balance { get; set; }

            [JsonPropertyName("currency")]
            public string Currency { get; set; } = string.Empty;
        }
    }
}
