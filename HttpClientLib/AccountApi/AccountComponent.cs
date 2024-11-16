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

        public AccountComponent(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
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

                Dictionary<string, object> accountBalances = null;
                string context = null;
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

    }
}
