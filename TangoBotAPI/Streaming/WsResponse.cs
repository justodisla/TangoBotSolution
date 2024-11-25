using System.Text.Json;

public class WsResponse 
{
    public string Type { get; set; }
    public int Channel { get; set; }
    public List<DataItem> Data { get; set; }

    public WsResponse(string message)
    {
        var jsonDocument = JsonDocument.Parse(message);
        var root = jsonDocument.RootElement;

        Type = root.GetProperty("type").GetString();
        Channel = root.GetProperty("channel").GetInt32();
        Data = new List<DataItem>();

        if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in dataElement.EnumerateArray())
            {
                try
                {

                    //If any of the properties time, open, high, low, close are not numbers, skip this data point
                    if (item.GetProperty("time").ValueKind != JsonValueKind.Number ||
                        item.GetProperty("open").ValueKind != JsonValueKind.Number ||
                        item.GetProperty("high").ValueKind != JsonValueKind.Number ||
                        item.GetProperty("low").ValueKind != JsonValueKind.Number ||
                        item.GetProperty("close").ValueKind != JsonValueKind.Number)
                    {
                        continue;
                    }

                    var dataItem = new DataItem
                    {
                        EventType = item.GetProperty("eventType").GetString(),
                        Time = item.GetProperty("time").GetInt64(),
                        Open = item.GetProperty("open").GetDecimal(),
                        High = item.GetProperty("high").GetDecimal(),
                        Low = item.GetProperty("low").GetDecimal(),
                        Close = item.GetProperty("close").GetDecimal(),
                        Volume = item.GetProperty("volume").GetDouble(),
                        Vwap = ResolveDoubleFromProperty(item, "vwap"),
                        BidVolume = ResolveDoubleFromProperty(item, "bidVolume"),
                        AskVolume = ResolveDoubleFromProperty(item, "askVolume"),
                        ImpVolatility = ResolveDoubleFromProperty(item, "impVolatility")
                    };
                    Data.Add(dataItem);
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
        }
    }

    private double ResolveDoubleFromProperty(JsonElement data, string propertyName)
    {
        try
        {
            var tmpValue = data.GetProperty(propertyName).ToString();
            if (tmpValue == "NaN")
            {
                return 0.0;
            }
            else
            {
                return data.GetProperty(propertyName).GetDouble();
            }
        }
        catch
        {
            return 0.0;
        }
    }

    public class DataItem
    {
        public string EventType { get; set; }
        public long Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public double Volume { get; set; }
        public double Vwap { get; set; }
        public double BidVolume { get; set; }
        public double AskVolume { get; set; }
        public double ImpVolatility { get; set; }
    }
}
