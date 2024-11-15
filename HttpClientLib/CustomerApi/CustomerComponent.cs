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
        private const string BaseCustomerUrl = "https://api.cert.tastyworks.com/customers/me";

        public CustomerComponent(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        /// <summary>
        /// Fetches customer information as a dictionary.
        /// </summary>
        public async Task<Dictionary<string, object>> GetCustomerInfoAsync()
        {
            string url = BaseCustomerUrl;
            var response = await SendGetRequestAsync(url);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON directly into a dictionary
                var customerData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                return customerData;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve customer information. Status code: {response?.StatusCode}");
                return null;
            }
        }
    }
}
