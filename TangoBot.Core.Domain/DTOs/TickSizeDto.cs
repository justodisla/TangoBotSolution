using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class TickSizeDto
    {
        public TickSizeDto() { }

        [JsonPropertyName("value")]
        public string ValueRaw { get; set; }

        [JsonIgnore]
        public double Value => double.TryParse(ValueRaw, out double parsedValue) ? parsedValue : 0;

        [JsonPropertyName("threshold")]
        public string ThresholdRaw { get; set; }

        [JsonIgnore]
        public double Threshold => double.TryParse(ThresholdRaw, out double parsedValue) ? parsedValue : 0;
    }
}
