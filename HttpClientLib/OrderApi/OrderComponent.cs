using HttpClientLib.TokenManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Toolkit;

namespace HttpClientLib.OrderApi
{
    public class OrderComponent : BaseApiComponent
    {
        private readonly string _baseUrl;

        public OrderComponent()
            : base()
        {
            _baseUrl = TangoBotServiceProvider.GetService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.ACTIVE_API_URL);

            //_baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public async Task<Order[]?> GetAccountOrdersAsync(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty", nameof(accountNumber));
            }

            var url = $"{_baseUrl}/accounts/{accountNumber}/orders";
            var response = await SendRequestAsync(url, HttpMethod.Get);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return OrderInfoDeserializer.DeserializeOrderArray(responseBody);
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account orders. Status code: {response?.StatusCode}");
                return null;
            }
        }

        public async Task<Order[]?> GetAccountLiveOrdersAsync(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty", nameof(accountNumber));
            }

            var url = $"{_baseUrl}/accounts/{accountNumber}/orders/live";
            var response = await SendRequestAsync(url, HttpMethod.Get);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return OrderInfoDeserializer.DeserializeOrderArray(responseBody);
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve account live orders. Status code: {response?.StatusCode}");
                return null;
            }
        }

        public async Task<Order> GetOrderByIdAsync(string accountNumber, int orderId)
        {
            var url = $"{_baseUrl}/accounts/{accountNumber}/orders/{orderId}";
            var response = await SendRequestAsync(url, HttpMethod.Get);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return OrderInfoDeserializer.DeserializeOrder(responseBody);
            }
            else
            {
                // Handle error response
                throw new HttpRequestException($"Failed to get order with ID {orderId}. Status code: " + ((response == null) ? "Unknown" : response.StatusCode));
            }
        }

        public async Task<OrderPostReport> PostEquityOrder(string accountNumber, OrderRequest orderRequest, bool isDryRun = true)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty", nameof(accountNumber));
            }

            var jsonContent = JsonSerializer.Serialize(orderRequest);
            Console.WriteLine($"Serialized JSON: {jsonContent}"); // Debugging step to print the serialized JSON

            var url = isDryRun ? $"{_baseUrl}/accounts/{accountNumber}/orders/dry-run" : $"{_baseUrl}/accounts/{accountNumber}/orders";
            var stringContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            //var response = await SendPostRequestAsync(url, stringContent);

            var response = await SendRequestAsync(url, HttpMethod.Post, stringContent);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return OrderInfoDeserializer.DeserializeDryRunReport(responseBody);
            }
            else
            {
                var responseBody = response == null ? "{}" : await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Error] Failed to post dry run order. Status code: {response?.StatusCode}. Response body: {responseBody}");
                // Handle error response
                throw new HttpRequestException($"Failed to post dry run order. Status code: {response?.StatusCode}");
            }
        }

        public async Task<Order> CancelOrderByIdAsync(string accountNumber, int orderId)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty", nameof(accountNumber));
            }

            var url = $"{_baseUrl}/accounts/{accountNumber}/orders/{orderId}";
            var response = await SendRequestAsync(url, HttpMethod.Delete);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return OrderInfoDeserializer.DeserializeOrder(responseBody);
            }
            else
            {
                var responseBody = response == null ? "{}" : await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Error] Failed to delete order with ID {orderId}. Status code: {response?.StatusCode}. Response body: {responseBody}");
                // Handle error response
                throw new HttpRequestException($"Failed to delete order with ID {orderId}. Status code: {response?.StatusCode}");
            }
        }

        //TODO: Implement ReplaceOrderByIdAsync method
        //TODO: Implement UpdateOrderByIdAsync method
        //TODO: Implement EditOrderDryRun method

    }

}

