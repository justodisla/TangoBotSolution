using System.Text.Json.Serialization;

public class PositionsResponse
{
    public Positions data { get; set; }
}

public class Positions
{
    public PositionItem[] items { get; set; }
}

public class PositionItem
{
    [JsonPropertyName("account-number")]
    public string AccountNumber { get; set; }

    [JsonPropertyName("instrument-type")]
    public string InstrumentType { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("underlying-symbol")]
    public string UnderlyingSymbol { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("average-open-price")]
    public string AverageOpenPrice { get; set; }

    [JsonPropertyName("close-price")]
    public string ClosePrice { get; set; }

    [JsonPropertyName("cost-effect")]
    public string CostEffect { get; set; }

    [JsonPropertyName("quantity-direction")]
    public string QuantityDirection { get; set; }

    [JsonPropertyName("realized-day-gain")]
    public string RealizedDayGain { get; set; }

    [JsonPropertyName("realized-today")]
    public string RealizedToday { get; set; }

    [JsonPropertyName("created-at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updated-at")]
    public string UpdatedAt { get; set; }

    // Additional fields can be added as needed
}
