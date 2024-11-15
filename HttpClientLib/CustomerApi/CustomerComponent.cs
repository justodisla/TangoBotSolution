using HttpClientLib.TokenManagement;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public class CustomerComponent : BaseApiComponent
    {
        private const string BaseCustomerUrl = "https://api.cert.tastyworks.com/customers/me";

        public CustomerComponent(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        /// <summary>
        /// Retrieves customer information.
        /// </summary>
        public async Task<CustomerInfo> GetCustomerInfoAsync()
        {
            var url = $"{BaseCustomerUrl}";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Customer information retrieved successfully.");

                var customerData = JsonSerializer.Deserialize<CustomerInfoResponse>(responseBody);
                return customerData?.data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve customer information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all accounts associated with the customer.
        /// </summary>
        public async Task<AccountInfo[]> GetCustomerAccountsAsync()
        {
            var url = $"{BaseCustomerUrl}/accounts";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Customer accounts retrieved successfully.");

                var accountData = JsonSerializer.Deserialize<AccountListResponse>(responseBody);
                return accountData?.data?.items?.Select(item => item.account).ToArray() ?? Array.Empty<AccountInfo>();
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve customer accounts. Status code: {response?.StatusCode}");
                return Array.Empty<AccountInfo>();
            }
        }

        /// <summary>
        /// Retrieves information for a specific account.
        /// </summary>
        public async Task<AccountInfo> GetAccountDetailsAsync(string accountNumber)
        {
            var url = $"{BaseCustomerUrl}/accounts/{accountNumber}";
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[Info] Account information retrieved successfully.");

                var accountDetails = JsonSerializer.Deserialize<AccountDetailsResponse>(responseBody);
                return accountDetails?.data;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account details. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
