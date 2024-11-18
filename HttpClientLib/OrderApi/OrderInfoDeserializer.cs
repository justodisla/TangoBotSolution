using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientLib.OrderApi
{
    public static class OrderInfoDeserializer
    {
        public static Order[] DeserializeOrderArray(string responseBody)
        {
            var response = JsonSerializer.Deserialize<OrderResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return response?.Data?.Items ?? Array.Empty<Order>();
        }

        public static Order DeserializeOrder(string responseBody)
        {
            var response = JsonSerializer.Deserialize<SingleOrderResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return response?.Data;
        }

        public static OrderPostReport DeserializeDryRunReport(string responseBody)
        {
            var response = JsonSerializer.Deserialize<OrderPostReport>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return response;
        }
    }

    public class OrderResponse
    {
        [JsonPropertyName("data")]
        public OrderData Data { get; set; }
    }

    public class OrderData
    {
        [JsonPropertyName("items")]
        public Order[] Items { get; set; }
    }

    public class SingleOrderResponse
    {
        [JsonPropertyName("data")]
        public Order Data { get; set; }
    }



    public class DryRunResponse
    {
        [JsonPropertyName("data")]
        public OrderPostReport Data { get; set; }
    }

}
