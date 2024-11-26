using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Toolkit;

namespace HttpClientLib
{
    public class MarketStatusChecker : ITTService
    {
        private readonly HttpClient? _httpClient;
        private readonly string? _apiKey;

        public MarketStatusChecker()
        {
            _httpClient = TangoBotServiceProvider.GetService<HttpClient>();
            _apiKey = TangoBotServiceProvider.GetService<IConfigurationProvider>()?.GetConfigurationValue(Constants.ALPHA_VANTAGE_API_KEY);
        }

        public async Task<bool> IsMarketOpenAsync()
        {
            string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=SPY&interval=1min&apikey={_apiKey}";

            try
            {
                if (_httpClient == null || string.IsNullOrEmpty(_apiKey))
                {
                    throw new Exception("HttpClient or API key is null");
                }
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                // Check if the market is open by looking at the latest timestamp
                var timeSeries = json["Time Series (1min)"];
                if (timeSeries != null)
                {
                    var latestTimestamp = timeSeries.First?.Path;

                    if (string.IsNullOrEmpty(latestTimestamp))
                    {
                        throw new Exception("Latest timestamp is null or empty");
                    }

                    var latestTime = DateTime.Parse(latestTimestamp);

                    // Assuming the market is open from 9:30 AM to 4:00 PM EST
                    var marketOpenTime = new TimeSpan(9, 30, 0);
                    var marketCloseTime = new TimeSpan(16, 0, 0);

                    if (latestTime.TimeOfDay >= marketOpenTime && latestTime.TimeOfDay <= marketCloseTime)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception in checking market status: {ex.Message}");
                return false;
            }
        }
    }
}
