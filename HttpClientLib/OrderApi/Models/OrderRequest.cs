using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HttpClientLib.OrderApi.Models
{
    public class OrderRequest
    {
        [JsonPropertyName("order-type")]
        public string OrderType { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("price-effect")]
        public string PriceEffect { get; set; }

        [JsonPropertyName("time-in-force")]
        public string TimeInForce { get; set; }

        [JsonPropertyName("legs")]
        public List<LegRequest> Legs { get; set; }
    }

    public class LegRequest
    {
        [JsonPropertyName("instrument-type")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}