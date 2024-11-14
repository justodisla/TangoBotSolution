using HttpClientLib.TokenProviding;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var tokenProvider = new TokenProvider(httpClient);
            var customerComponent = new CustomerComponent(httpClient, tokenProvider);

            // Test 1: Get Customer Info
            Console.WriteLine("[Test 1] Fetching customer information...");
            var customerInfo = await customerComponent.GetCustomerInfoAsync();
            if (customerInfo != null)
            {
                Console.WriteLine("Customer Info:");
                Console.WriteLine($"ID: {customerInfo.Id}");
                Console.WriteLine($"Name: {customerInfo.FirstName} {customerInfo.LastName}");
                Console.WriteLine($"Mobile: {customerInfo.MobilePhoneNumber}");
                Console.WriteLine($"Birth Date: {customerInfo.BirthDate}");
                Console.WriteLine($"Email: {customerInfo.Email}");
                Console.WriteLine("Permitted Account Types:");
                foreach (var accountType in customerInfo.PermittedAccountTypes)
                {
                    Console.WriteLine($"  - {accountType.Name} (Tax-Advantaged: {accountType.IsTaxAdvantaged})");
                }
            }
            else
            {
                Console.WriteLine("[Error] Failed to retrieve customer information.");
            }

            // Test 2: Get Customer Accounts
            Console.WriteLine("\n[Test 2] Fetching customer accounts...");
            var accounts = await customerComponent.GetCustomerAccountsAsync();
            if (accounts != null && accounts.Length > 0)
            {
                foreach (var account in accounts)
                {
                    Console.WriteLine("Account Info:");
                    Console.WriteLine($"Account Number: {account.AccountNumber}");
                    Console.WriteLine($"Account Type: {account.AccountTypeName}");
                    Console.WriteLine($"Day Trader Status: {account.DayTraderStatus}");
                    Console.WriteLine($"Investment Objective: {account.InvestmentObjective}");
                    Console.WriteLine($"Opened At: {account.OpenedAt}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("[Error] Failed to retrieve customer accounts.");
            }

            // Test 3: Get Specific Account Details
            if (accounts != null && accounts.Length > 0)
            {
                string accountNumber = accounts[0].AccountNumber;
                Console.WriteLine($"\n[Test 3] Fetching details for account {accountNumber}...");
                //var accountDetails = await customerComponent.GetAccountDetailsAsync(account
                var accountDetails = await customerComponent.GetAccountDetailsAsync(accountNumber);
                if (accountDetails != null)
                {
                    Console.WriteLine("Account Details:");
                    Console.WriteLine($"Account Number: {accountDetails.AccountNumber}");
                    Console.WriteLine($"Account Type: {accountDetails.AccountTypeName}");
                    Console.WriteLine($"Day Trader Status: {accountDetails.DayTraderStatus}");
                    Console.WriteLine($"Investment Objective: {accountDetails.InvestmentObjective}");
                    Console.WriteLine($"Margin or Cash: {accountDetails.MarginOrCash}");
                    Console.WriteLine($"Suitable Options Level: {accountDetails.SuitableOptionsLevel}");
                    Console.WriteLine($"Nickname: {accountDetails.Nickname}");
                    Console.WriteLine($"Authority Level: {accountDetails.AuthorityLevel}");
                    Console.WriteLine($"Opened At: {accountDetails.OpenedAt}");
                    Console.WriteLine($"Is Closed: {accountDetails.IsClosed}");
                    Console.WriteLine($"Is Firm Error: {accountDetails.IsFirmError}");
                    Console.WriteLine($"Is Firm Proprietary: {accountDetails.IsFirmProprietary}");
                    Console.WriteLine($"Is Foreign: {accountDetails.IsForeign}");
                    Console.WriteLine($"Is Futures Approved: {accountDetails.IsFuturesApproved}");
                    Console.WriteLine($"Is Test Drive: {accountDetails.IsTestDrive}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("[Error] Failed to retrieve account details.");
                }
            }
            else
            {
                Console.WriteLine("[Error] No accounts found for testing account details retrieval.");
            }

            Console.WriteLine("\n[Info] Testing completed.");
        }
    }
}
