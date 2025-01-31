using System;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class PositionDto
    {
        public PositionDto() { }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("instrument-type")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("underlying-symbol")]
        public string UnderlyingSymbol { get; set; }

        [JsonPropertyName("quantity")]
        public string QuantityRaw { get; set; }
        [JsonIgnore]
        public int Quantity => int.TryParse(QuantityRaw, out int value) ? value : 0;

        [JsonPropertyName("quantity-direction")]
        public string QuantityDirection { get; set; }

        [JsonPropertyName("close-price")]
        public string ClosePriceRaw { get; set; }
        [JsonIgnore]
        public double ClosePrice => double.TryParse(ClosePriceRaw, out double value) ? value : 0;

        [JsonPropertyName("average-open-price")]
        public string AverageOpenPriceRaw { get; set; }
        [JsonIgnore]
        public double AverageOpenPrice => double.TryParse(AverageOpenPriceRaw, out double value) ? value : 0;

        [JsonPropertyName("average-yearly-market-close-price")]
        public string AverageYearlyMarketClosePriceRaw { get; set; }
        [JsonIgnore]
        public double AverageYearlyMarketClosePrice => double.TryParse(AverageYearlyMarketClosePriceRaw, out double value) ? value : 0;

        [JsonPropertyName("average-daily-market-close-price")]
        public string AverageDailyMarketClosePriceRaw { get; set; }
        [JsonIgnore]
        public double AverageDailyMarketClosePrice => double.TryParse(AverageDailyMarketClosePriceRaw, out double value) ? value : 0;

        [JsonPropertyName("multiplier")]
        public int Multiplier { get; set; }

        [JsonPropertyName("cost-effect")]
        public string CostEffect { get; set; }

        [JsonPropertyName("is-suppressed")]
        public bool IsSuppressed { get; set; }

        [JsonPropertyName("is-frozen")]
        public bool IsFrozen { get; set; }

        [JsonPropertyName("restricted-quantity")]
        public string RestrictedQuantityRaw { get; set; }
        [JsonIgnore]
        public double RestrictedQuantity => double.TryParse(RestrictedQuantityRaw, out double value) ? value : 0;

        [JsonPropertyName("realized-day-gain")]
        public string RealizedDayGainRaw { get; set; }
        [JsonIgnore]
        public double RealizedDayGain => double.TryParse(RealizedDayGainRaw, out double value) ? value : 0;

        [JsonPropertyName("realized-day-gain-effect")]
        public string RealizedDayGainEffect { get; set; }

        [JsonPropertyName("realized-day-gain-date")]
        public DateTime RealizedDayGainDate { get; set; }

        [JsonPropertyName("realized-today")]
        public string RealizedTodayRaw { get; set; }
        [JsonIgnore]
        public double RealizedToday => double.TryParse(RealizedTodayRaw, out double value) ? value : 0;

        [JsonPropertyName("realized-today-effect")]
        public string RealizedTodayEffect { get; set; }

        [JsonPropertyName("realized-today-date")]
        public DateTime RealizedTodayDate { get; set; }

        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated-at")]
        public DateTime UpdatedAt { get; set; }
    }
}
