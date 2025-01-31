using System;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountBalanceDto
    {
        public AccountBalanceDto() { }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("available-trading-funds")]
        public string AvailableTradingFundsRaw { get; set; }
        [JsonIgnore]
        public double AvailableTradingFunds => double.TryParse(AvailableTradingFundsRaw, out double value) ? value : 0;

        [JsonPropertyName("bond-margin-requirement")]
        public string BondMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double BondMarginRequirement => double.TryParse(BondMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("cash-available-to-withdraw")]
        public string CashAvailableToWithdrawRaw { get; set; }
        [JsonIgnore]
        public double CashAvailableToWithdraw => double.TryParse(CashAvailableToWithdrawRaw, out double value) ? value : 0;

        [JsonPropertyName("cash-balance")]
        public string CashBalanceRaw { get; set; }
        [JsonIgnore]
        public double CashBalance => double.TryParse(CashBalanceRaw, out double value) ? value : 0;

        [JsonPropertyName("closed-loop-available-balance")]
        public string ClosedLoopAvailableBalanceRaw { get; set; }
        [JsonIgnore]
        public double ClosedLoopAvailableBalance => double.TryParse(ClosedLoopAvailableBalanceRaw, out double value) ? value : 0;

        [JsonPropertyName("cryptocurrency-margin-requirement")]
        public string CryptocurrencyMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double CryptocurrencyMarginRequirement => double.TryParse(CryptocurrencyMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("day-equity-call-value")]
        public string DayEquityCallValueRaw { get; set; }
        [JsonIgnore]
        public double DayEquityCallValue => double.TryParse(DayEquityCallValueRaw, out double value) ? value : 0;

        [JsonPropertyName("day-trade-excess")]
        public string DayTradeExcessRaw { get; set; }
        [JsonIgnore]
        public double DayTradeExcess => double.TryParse(DayTradeExcessRaw, out double value) ? value : 0;

        [JsonPropertyName("day-trading-buying-power")]
        public string DayTradingBuyingPowerRaw { get; set; }
        [JsonIgnore]
        public double DayTradingBuyingPower => double.TryParse(DayTradingBuyingPowerRaw, out double value) ? value : 0;

        [JsonPropertyName("day-trading-call-value")]
        public string DayTradingCallValueRaw { get; set; }
        [JsonIgnore]
        public double DayTradingCallValue => double.TryParse(DayTradingCallValueRaw, out double value) ? value : 0;

        [JsonPropertyName("derivative-buying-power")]
        public string DerivativeBuyingPowerRaw { get; set; }
        [JsonIgnore]
        public double DerivativeBuyingPower => double.TryParse(DerivativeBuyingPowerRaw, out double value) ? value : 0;

        [JsonPropertyName("equity-buying-power")]
        public string EquityBuyingPowerRaw { get; set; }
        [JsonIgnore]
        public double EquityBuyingPower => double.TryParse(EquityBuyingPowerRaw, out double value) ? value : 0;

        [JsonPropertyName("equity-offering-margin-requirement")]
        public string EquityOfferingMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double EquityOfferingMarginRequirement => double.TryParse(EquityOfferingMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("futures-margin-requirement")]
        public string FuturesMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double FuturesMarginRequirement => double.TryParse(FuturesMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("long-bond-value")]
        public string LongBondValueRaw { get; set; }
        [JsonIgnore]
        public double LongBondValue => double.TryParse(LongBondValueRaw, out double value) ? value : 0;

        [JsonPropertyName("long-cryptocurrency-value")]
        public string LongCryptocurrencyValueRaw { get; set; }
        [JsonIgnore]
        public double LongCryptocurrencyValue => double.TryParse(LongCryptocurrencyValueRaw, out double value) ? value : 0;

        [JsonPropertyName("long-derivative-value")]
        public string LongDerivativeValueRaw { get; set; }
        [JsonIgnore]
        public double LongDerivativeValue => double.TryParse(LongDerivativeValueRaw, out double value) ? value : 0;

        [JsonPropertyName("long-equity-value")]
        public string LongEquityValueRaw { get; set; }
        [JsonIgnore]
        public double LongEquityValue => double.TryParse(LongEquityValueRaw, out double value) ? value : 0;

        [JsonPropertyName("long-futures-value")]
        public string LongFuturesValueRaw { get; set; }
        [JsonIgnore]
        public double LongFuturesValue => double.TryParse(LongFuturesValueRaw, out double value) ? value : 0;

        [JsonPropertyName("maintenance-call-value")]
        public string MaintenanceCallValueRaw { get; set; }
        [JsonIgnore]
        public double MaintenanceCallValue => double.TryParse(MaintenanceCallValueRaw, out double value) ? value : 0;

        [JsonPropertyName("maintenance-requirement")]
        public string MaintenanceRequirementRaw { get; set; }
        [JsonIgnore]
        public double MaintenanceRequirement => double.TryParse(MaintenanceRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("margin-equity")]
        public string MarginEquityRaw { get; set; }
        [JsonIgnore]
        public double MarginEquity => double.TryParse(MarginEquityRaw, out double value) ? value : 0;

        [JsonPropertyName("net-liquidating-value")]
        public string NetLiquidatingValueRaw { get; set; }
        [JsonIgnore]
        public double NetLiquidatingValue => double.TryParse(NetLiquidatingValueRaw, out double value) ? value : 0;

        [JsonPropertyName("snapshot-date")]
        public DateTime SnapshotDate { get; set; }

        [JsonPropertyName("updated-at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("futures-overnight-margin-requirement")]
        public string FuturesOvernightMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double FuturesOvernightMarginRequirement => double.TryParse(FuturesOvernightMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("futures-intraday-margin-requirement")]
        public string FuturesIntradayMarginRequirementRaw { get; set; }
        [JsonIgnore]
        public double FuturesIntradayMarginRequirement => double.TryParse(FuturesIntradayMarginRequirementRaw, out double value) ? value : 0;

        [JsonPropertyName("maintenance-excess")]
        public string MaintenanceExcessRaw { get; set; }
        [JsonIgnore]
        public double MaintenanceExcess => double.TryParse(MaintenanceExcessRaw, out double value) ? value : 0;

        [JsonPropertyName("pending-margin-interest")]
        public string PendingMarginInterestRaw { get; set; }
        [JsonIgnore]
        public double PendingMarginInterest => double.TryParse(PendingMarginInterestRaw, out double value) ? value : 0;

        [JsonPropertyName("effective-cryptocurrency-buying-power")]
        public string EffectiveCryptocurrencyBuyingPowerRaw { get; set; }
        [JsonIgnore]
        public double EffectiveCryptocurrencyBuyingPower => double.TryParse(EffectiveCryptocurrencyBuyingPowerRaw, out double value) ? value : 0;
    }
}
