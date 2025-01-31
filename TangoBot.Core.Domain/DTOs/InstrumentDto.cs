using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class InstrumentDto
    {
        public InstrumentDto() { }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("instrument-type")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }

        [JsonPropertyName("short-description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("is-index")]
        public bool IsIndex { get; set; }

        [JsonPropertyName("listed-market")]
        public string ListedMarket { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("lendability")]
        public string Lendability { get; set; }

        [JsonPropertyName("borrow-rate")]
        public string BorrowRateRaw { get; set; }

        [JsonIgnore]
        public double BorrowRate => double.TryParse(BorrowRateRaw, out double value) ? value : 0;

        [JsonPropertyName("market-time-instrument-collection")]
        public string MarketTimeInstrumentCollection { get; set; }

        [JsonPropertyName("is-closing-only")]
        public bool IsClosingOnly { get; set; }

        [JsonPropertyName("is-options-closing-only")]
        public bool IsOptionsClosingOnly { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("is-fractional-quantity-eligible")]
        public bool IsFractionalQuantityEligible { get; set; }

        [JsonPropertyName("is-illiquid")]
        public bool IsIlliquid { get; set; }

        [JsonPropertyName("is-etf")]
        public bool IsETF { get; set; }

        [JsonPropertyName("streamer-symbol")]
        public string StreamerSymbol { get; set; }

        [JsonPropertyName("tick-sizes")]
        public List<TickSizeDto> TickSizes { get; set; } = new();

        [JsonPropertyName("option-tick-sizes")]
        public List<TickSizeDto> OptionTickSizes { get; set; } = new();
    }
}
