using HttpClientLib;
using HttpClientLib.TokenManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public class AccountComponent : BaseApiComponent
    {
        private const string BaseAccountUrl = "https://api.cert.tastyworks.com/accounts";

        public AccountComponent()
            : base()
        {
        }

        /// <summary>
        /// Fetches account information as a dictionary based on the account number.
        /// </summary>
        public async Task<Dictionary<string, object>> GetAccountBalancesAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/balances";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                Dictionary<string, object>? accountBalances = null;
                string? context = null;
                bool successful = AccountInfoDeserializer.DeserializeAccountBalances(responseBody, ref accountBalances, ref context);

                if (successful)
                {
                    return accountBalances;
                }
                else
                {
                    Console.WriteLine("[Error] Failed to deserialize account information.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches balance snapshots for a specific account.
        /// </summary>
        public async Task<Dictionary<string, object>[]?> GetBalanceSnapshotAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/balance-snapshots";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                Dictionary<string, object>[]? balanceSnapshots = null;
                string? context = null;
                bool successful = AccountInfoDeserializer.DeserializeBalanceSnapshots(responseBody, ref balanceSnapshots, ref context);

                if (successful)
                {
                    return balanceSnapshots;
                }
                else
                {
                    Console.WriteLine("[Error] Failed to deserialize balance snapshots.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve balance snapshots. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches account positions for a specific account.
        /// </summary>
        public async Task<Dictionary<string, object>[]> GetAccountPositionsAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/positions";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                Dictionary<string, object>[]? accountPositions = null;
                string? context = null;
                bool successful = AccountInfoDeserializer.DeserializeAccountPositions(responseBody, ref accountPositions, ref context);

                if (successful)
                {
                    return accountPositions;
                }
                else
                {
                    Console.WriteLine("[Error] Failed to deserialize account positions.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account positions. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
