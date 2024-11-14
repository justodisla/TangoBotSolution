using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HttpClientLib.TokenManagement;
using TangoBot.HttpClientLib;

namespace HttpClientLib.AccountApi
{
    public class AccountComponent : BaseApiComponent
    {
        private const string BaseAccountUrl = "https://api.cert.tastyworks.com/accounts";

        public AccountComponent(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        /// <summary>
        /// Fetches account balance information for a specific account.
        /// </summary>
        public async Task<AccountBalance> GetAccountBalanceAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/balances";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Account balance information retrieved successfully.");

                var balanceData = JsonSerializer.Deserialize<AccountBalanceResponse>(responseBody);
                return balanceData?.data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account balance information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches balance snapshots for a specific account.
        /// </summary>
        public async Task<BalanceSnapshots> GetBalanceSnapshotsAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/balance-snapshots";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Balance snapshots retrieved successfully.");

                var snapshotsData = JsonSerializer.Deserialize<BalanceSnapshotsResponse>(responseBody);
                return snapshotsData?.data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve balance snapshots. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches positions for a specific account.
        /// </summary>
        public async Task<Positions> GetPositionsAsync(string accountNumber)
        {
            string url = $"{BaseAccountUrl}/{accountNumber}/positions";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Account positions retrieved successfully.");

                var positionsData = JsonSerializer.Deserialize<PositionsResponse>(responseBody);
                return positionsData?.data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account positions. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
