using HttpClientLib;
using HttpClientLib.CustomerApi;
using HttpClientLib.TokenManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public class CustomerComponent : BaseApiComponent
    {
        private const string BaseCustomersUrl = "https://api.cert.tastyworks.com/customers";
        private const string BaseCustomerUrl = "https://api.cert.tastyworks.com/customers/me";
        private const string CustomerAccountsUrl = "https://api.cert.tastyworks.com/customers/me/accounts";

        public CustomerComponent()
            : base()
        {
        }

        /// <summary>
        /// Fetches customer information as a strongly typed object.
        /// </summary>
        public async Task<Customer> GetCustomerInfoAsync()
        {
            string url = BaseCustomerUrl;
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into a Customer object
                var customer = JsonSerializer.Deserialize<Customer>(responseBody);

                return customer;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve customer information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches customer accounts information as a list of strongly typed objects.
        /// </summary>
        public async Task<List<AccountInfo>> GetCustomerAccountsAsync()
        {
            string url = CustomerAccountsUrl;
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into a list of AccountInfo objects
                var accountListResponse = JsonSerializer.Deserialize<AccountListResponse>(responseBody);
                var accounts = accountListResponse?.data?.items?.ConvertAll(item => item.account);

                return accounts;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve customer accounts information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches a specific account information for a given customer ID and account number.
        /// </summary>
        public async Task<Account> GetAccountAsync(string customerId, string accountNumber)
        {
            string url = $"{BaseCustomersUrl}/{customerId}/accounts/{accountNumber}";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into an Account object using AccountInfoDeserializer
                var account = AccountInfoDeserializer.DeserializeAccount(responseBody);

                return account;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account information. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
