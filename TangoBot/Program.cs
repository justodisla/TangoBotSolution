using HttpClientLib.AccountApi;
using HttpClientLib.TokenManagement;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            TokenProvider tokenProvider = new TokenProvider(httpClient);
            AccountComponent accountComponent = new AccountComponent(httpClient, tokenProvider);

            string accountNumber = "5WU34986";

            // Test GetAccountBalanceAsync
            var accountBalance = await accountComponent.GetAccountBalanceAsync(accountNumber);
            Console.WriteLine($"Account Balance: {accountBalance?.NetLiquidatingValue}");

            // Test GetBalanceSnapshotsAsync
            var balanceSnapshots = await accountComponent.GetBalanceSnapshotsAsync(accountNumber);
            Console.WriteLine($"Balance Snapshots: {balanceSnapshots?.items.Length}");

            // Test GetPositionsAsync
            var positions = await accountComponent.GetPositionsAsync(accountNumber);
            Console.WriteLine($"Positions: {positions?.items.Length}");
        }
    }
}
