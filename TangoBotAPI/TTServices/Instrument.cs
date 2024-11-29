using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TangoBot.API.TTServices
{
    public class Instrument
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("bypass-manual-review")]
        public bool BypassManualReview { get; set; }

        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("instrument-type")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("is-closing-only")]
        public bool IsClosingOnly { get; set; }

        [JsonPropertyName("is-etf")]
        public bool IsEtf { get; set; }

        [JsonPropertyName("is-fraud-risk")]
        public bool IsFraudRisk { get; set; }

        [JsonPropertyName("is-illiquid")]
        public bool IsIlliquid { get; set; }

        [JsonPropertyName("is-index")]
        public bool IsIndex { get; set; }

        [JsonPropertyName("is-options-closing-only")]
        public bool IsOptionsClosingOnly { get; set; }

        [JsonPropertyName("lendability")]
        public string Lendability { get; set; }

        [JsonPropertyName("listed-market")]
        public string ListedMarket { get; set; }

        [JsonPropertyName("market-time-instrument-collection")]
        public string MarketTimeInstrumentCollection { get; set; }

        [JsonPropertyName("short-description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("streamer-symbol")]
        public string StreamerSymbol { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("option-tick-sizes")]
        public List<OptionTickSize> OptionTickSizes { get; set; }

        [JsonPropertyName("tick-sizes")]
        public List<TickSize> TickSizes { get; set; }
    }

    public class OptionTickSize
    {
        [JsonPropertyName("threshold")]
        public string Threshold { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class TickSize
    {
        [JsonPropertyName("threshold")]
        public string Threshold { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
