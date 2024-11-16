using System;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;
using HttpClientLib.TokenManagement;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create HttpClient instance
            using var httpClient = new HttpClient();

            // Create a TokenProvider instance (assuming you have a suitable constructor)
            var tokenProvider = new TokenProvider(httpClient);

            // Create AccountComponent instance
            var accountComponent = new AccountComponent(httpClient, tokenProvider);

            // Test account number
            string accountNumber = "5WU34986";

            // Fetch account information
            var accountInfo = await accountComponent.GetAccountBalancesAsync(accountNumber);

            // Print account information
            if (accountInfo != null)
            {
                Console.WriteLine("Account Information:");
                foreach (var kvp in accountInfo)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve account information.");
            }
        }
    }
}
