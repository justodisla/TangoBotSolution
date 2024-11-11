using TangoBot.HttpClientLib;
using TangoBot.DatabaseLib;
using TangoBot.IndicatorsLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting TangoBot...");

            // Initialize database
            var dbPath = "TangoBotData.db";
            var dbManager = new DatabaseManager(dbPath);
            dbManager.InitializeDatabase();

            // Initialize API client
            var apiKey = "YOUR_API_KEY";
            var apiBaseUrl = "https://api.tastytrade.com";
            var apiClient = new TastyTradeApiClient(apiBaseUrl, apiKey);

            // Test API connection
            var symbol = "SPY";
            var marketData = await apiClient.GetMarketDataAsync(symbol);
            Console.WriteLine($"Market data for {symbol}: {marketData}");

            // Test SMA calculation
            var smaCalculator = new SmaCalculator();
            var samplePrices = new List<decimal> { 100, 102, 101, 103, 104 };
            var sma = smaCalculator.CalculateSMA(samplePrices, 3);
            Console.WriteLine($"Sample SMA: {sma}");
        }
    }
}
