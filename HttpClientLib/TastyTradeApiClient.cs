using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public class TastyTradeApiClient
    {
        private readonly HttpClient _httpClient;

        public TastyTradeApiClient(string baseUrl, string apiKey)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetMarketDataAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"/marketdata/{symbol}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // You can add more methods as needed, e.g., for order placement or account information retrieval.
    }
}
