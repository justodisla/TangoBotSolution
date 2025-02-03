using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TangoBotTrainerLib.Data
{
    public class MarketData
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<JObject> FetchHistoricalData(string symbol, string apiKey)
        {
            string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}&outputsize=compact";
            string response = await client.GetStringAsync(url);
            return JObject.Parse(response);
        }
    }
}
