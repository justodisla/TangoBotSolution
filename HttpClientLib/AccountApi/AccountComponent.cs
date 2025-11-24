using HttpClientLib;
using HttpClientLib.AccountApi.Observer;
using HttpClientLib.OrderApi.Observer;
using HttpClientLib.TokenManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.API.Configuration;
using TangoBot.API.Toolkit;
using TangoBot.API.TTServices;
using TangoBot.DependecyInjection;

namespace TangoBot.HttpClientLib.AccountApi
{
    public class AccountComponent : BaseApiComponent, IAccountComponent
    {
        private readonly string _baseAccountUrl;

        public AccountComponent()
            : base()
        {
            // Resolve the base URL dynamically from the configuration provider
            var configurationProvider = TangoBotServiceLocator.GetSingletonService<IConfigurationProvider>()
                ?? throw new Exception("ConfigurationProvider is not available.");
            _baseAccountUrl = $"{configurationProvider.GetConfigurationValue(Constants.ACTIVE_API_URL)}/accounts";

            Subscribe(new AccountObserver());
        }

        /// <summary>
        /// Fetches account information as a dictionary based on the account number.
        /// </summary>
        public async Task<Dictionary<string, object>> GetAccountBalancesAsync(string accountNumber)
        {
            string url = $"{_baseAccountUrl}/{accountNumber}/balances";
            var response = await SendRequestAsync(url, HttpMethod.Get);

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
            string url = $"{_baseAccountUrl}/{accountNumber}/balance-snapshots";
            var response = await SendRequestAsync(url, HttpMethod.Get);

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
            string url = $"{_baseAccountUrl}/{accountNumber}/positions";
            var response = await SendRequestAsync(url, HttpMethod.Get);

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
