using HttpClientLib;
using HttpClientLib.InstrumentApi;
using HttpClientLib.TokenManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBot.API.TTServices;

namespace TangoBot.HttpClientLib.InstrumentApi
{
    public class InstrumentComponent : BaseApiComponent, IInstrumentComponent
    {
        private const string BaseInstrumentUrl = "https://api.cert.tastyworks.com/instruments";

        public InstrumentComponent()
            : base()
        {
        }

        /// <summary>
        /// Fetches instrument information for a given symbol.
        /// </summary>
        public async Task<Instrument> GetInstrumentBySymbolAsync(string symbol)
        {
            string url = $"{BaseInstrumentUrl}/equities/{symbol}";
            var response = await SendRequestAsync(url, HttpMethod.Get);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into an Instrument object using InstrumentInfoDeserializer
                var instrument = InstrumentInfoDeserializer.DeserializeInstrument(responseBody);

                return instrument;
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve instrument information. Status code: {response?.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// Fetches a list of active instruments.
        /// </summary>
        public async Task<List<Instrument>> GetActiveInstrumentsAsync()
        {
            string url = $"{BaseInstrumentUrl}/equities/active";
            var response = await SendRequestAsync(url, HttpMethod.Get);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into a list of Instrument objects
                var activeInstrumentsResponse = JsonSerializer.Deserialize<ActiveInstrumentsResponse>(responseBody);

                return activeInstrumentsResponse?.Data?.Items ?? new List<Instrument>();
            }
            else
            {
                Console.WriteLine($"[Error] Failed to retrieve active instruments. Status code: {response?.StatusCode}");
                return new List<Instrument>();
            }
        }
    }

    public class ActiveInstrumentsResponse
    {
        [JsonPropertyName("data")]
        public ActiveInstrumentsData Data { get; set; }
    }

    public class ActiveInstrumentsData
    {
        [JsonPropertyName("items")]
        public List<Instrument> Items { get; set; }
    }
}
