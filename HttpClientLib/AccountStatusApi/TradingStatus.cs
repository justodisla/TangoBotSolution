using System;
using System.Text.Json.Serialization;

public class TradingStatus
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("account-number")]
    public string AccountNumber { get; set; }

    [JsonPropertyName("are-deep-itm-carry-options-enabled")]
    public bool AreDeepItmCarryOptionsEnabled { get; set; }

    [JsonPropertyName("are-far-otm-net-options-restricted")]
    public bool AreFarOtmNetOptionsRestricted { get; set; }

    [JsonPropertyName("are-options-values-restricted-to-nlv")]
    public bool AreOptionsValuesRestrictedToNlv { get; set; }

    [JsonPropertyName("are-single-tick-expiring-hedges-ignored")]
    public bool AreSingleTickExpiringHedgesIgnored { get; set; }

    [JsonPropertyName("autotrade-account-type")]
    public string AutotradeAccountType { get; set; }

    [JsonPropertyName("clearing-account-number")]
    public string ClearingAccountNumber { get; set; }

    [JsonPropertyName("clearing-aggregation-identifier")]
    public string ClearingAggregationIdentifier { get; set; }

    [JsonPropertyName("cmta-override")]
    public int CmtaOverride { get; set; }

    [JsonPropertyName("day-trade-count")]
    public int DayTradeCount { get; set; }

    [JsonPropertyName("enhanced-fraud-safeguards-enabled-at")]
    public DateTime? EnhancedFraudSafeguardsEnabledAt { get; set; }

    [JsonPropertyName("equities-margin-calculation-type")]
    public string EquitiesMarginCalculationType { get; set; }

    [JsonPropertyName("ext-crm-id")]
    public string ExtCrmId { get; set; }

    [JsonPropertyName("fee-schedule-name")]
    public string FeeScheduleName { get; set; }

    [JsonPropertyName("futures-margin-rate-multiplier")]
    public double FuturesMarginRateMultiplier { get; set; }

    [JsonPropertyName("has-intraday-equities-margin")]
    public bool HasIntradayEquitiesMargin { get; set; }

    [JsonPropertyName("is-aggregated-at-clearing")]
    public bool IsAggregatedAtClearing { get; set; }

    [JsonPropertyName("is-closed")]
    public bool IsClosed { get; set; }

    [JsonPropertyName("is-closing-only")]
    public bool IsClosingOnly { get; set; }

    [JsonPropertyName("is-cryptocurrency-closing-only")]
    public bool IsCryptocurrencyClosingOnly { get; set; }

    [JsonPropertyName("is-cryptocurrency-enabled")]
    public bool IsCryptocurrencyEnabled { get; set; }

    [JsonPropertyName("is-equity-offering-closing-only")]
    public bool IsEquityOfferingClosingOnly { get; set; }

    [JsonPropertyName("is-equity-offering-enabled")]
    public bool IsEquityOfferingEnabled { get; set; }

    [JsonPropertyName("is-frozen")]
    public bool IsFrozen { get; set; }

    [JsonPropertyName("is-full-equity-margin-required")]
    public bool IsFullEquityMarginRequired { get; set; }

    [JsonPropertyName("is-futures-closing-only")]
    public bool IsFuturesClosingOnly { get; set; }

    [JsonPropertyName("is-futures-enabled")]
    public bool IsFuturesEnabled { get; set; }

    [JsonPropertyName("is-futures-intra-day-enabled")]
    public bool IsFuturesIntraDayEnabled { get; set; }

    [JsonPropertyName("is-in-day-trade-equity-maintenance-call")]
    public bool IsInDayTradeEquityMaintenanceCall { get; set; }

    [JsonPropertyName("is-in-margin-call")]
    public bool IsInMarginCall { get; set; }

    [JsonPropertyName("is-pattern-day-trader")]
    public bool IsPatternDayTrader { get; set; }

    [JsonPropertyName("is-portfolio-margin-enabled")]
    public bool IsPortfolioMarginEnabled { get; set; }

    [JsonPropertyName("is-risk-reducing-only")]
    public bool IsRiskReducingOnly { get; set; }

    [JsonPropertyName("is-roll-the-day-forward-enabled")]
    public bool IsRollTheDayForwardEnabled { get; set; }

    [JsonPropertyName("is-small-notional-futures-intra-day-enabled")]
    public bool IsSmallNotionalFuturesIntraDayEnabled { get; set; }

    [JsonPropertyName("options-level")]
    public string OptionsLevel { get; set; }

    [JsonPropertyName("pdt-reset-on")]
    public DateTime? PdtResetOn { get; set; }

    [JsonPropertyName("short-calls-enabled")]
    public bool ShortCallsEnabled { get; set; }

    [JsonPropertyName("small-notional-futures-margin-rate-multiplier")]
    public double SmallNotionalFuturesMarginRateMultiplier { get; set; }

    [JsonPropertyName("updated-at")]
    public DateTime? UpdatedAt { get; set; }
}
