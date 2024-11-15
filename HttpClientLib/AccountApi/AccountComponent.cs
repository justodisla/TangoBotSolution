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
        public async Task<Dictionary<string, object>> GetAccountInfoAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/balances";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into AccountResponse
                var accountResponse = JsonSerializer.Deserialize<AccountResponse>(responseBody);

                // Return the data dictionary
                return accountResponse?.Data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account information. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
