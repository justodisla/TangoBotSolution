using System.Text.Json;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountBalanceDto : AbstractDTO
    {
        public AccountBalanceDto(JsonDocument jsonDocument) : base(jsonDocument) { }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; }
        [JsonPropertyName("available-trading-funds")]
        public double AvailableTradingFunds { get; set; }
        [JsonPropertyName("bond-margin-requirement")]
        public double BondMarginRequirement { get; set; }
        [JsonPropertyName("cash-available-to-withdraw")]
        public double CashAvailableToWithdraw { get; set; }
        [JsonPropertyName("cash-balance")]
        public double CashBalance { get; set; }
        [JsonPropertyName("cash-settle-balance")]
        public double CashSettleBalance { get; set; }
        [JsonPropertyName("closed-loop-available-balance")]
        public double ClosedLoopAvailableBalance { get; set; }
        [JsonPropertyName("cryptocurrency-margin-requirement")]
        public double CryptocurrencyMarginRequirement { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("day-equity-call-value")]
        public double DayEquityCallValue { get; set; }
        [JsonPropertyName("day-trade-excess")]
        public double DayTradeExcess { get; set; }
        [JsonPropertyName("day-trading-buying-power")]
        public double DayTradingBuyingPower { get; set; }
        [JsonPropertyName("day-trading-call-value")]
        public double DayTradingCallValue { get; set; }
        [JsonPropertyName("derivative-buying-power")]
        public double DerivativeBuyingPower { get; set; }
        [JsonPropertyName("equity-buying-power")]
        public double EquityBuyingPower { get; set; }
        [JsonPropertyName("equity-offering-margin-requirement")]
        public double EquityOfferingMarginRequirement { get; set; }
        [JsonPropertyName("fixed-income-security-margin-requirement")]
        public double FixedIncomeSecurityMarginRequirement { get; set; }
        [JsonPropertyName("futures-margin-requirement")]
        public double FuturesMarginRequirement { get; set; }
        [JsonPropertyName("long-bond-value")]
        public double LongBondValue { get; set; }
        [JsonPropertyName("long-cryptocurrency-value")]
        public double LongCryptocurrencyValue { get; set; }
        [JsonPropertyName("long-derivative-value")]
        public double LongDerivativeValue { get; set; }
        [JsonPropertyName("long-equity-value")]
        public double LongEquityValue { get; set; }
        [JsonPropertyName("long-fixed-income-security-value")]
        public double LongFixedIncomeSecurityValue { get; set; }
        [JsonPropertyName("long-futures-derivative-value")]
        public double LongFuturesDerivativeValue { get; set; }
        [JsonPropertyName("long-futures-value")]
        public double LongFuturesValue { get; set; }
        [JsonPropertyName("long-margineable-value")]
        public double LongMargineableValue { get; set; }
        [JsonPropertyName("maintenance-call-value")]
        public double MaintenanceCallValue { get; set; }
        [JsonPropertyName("maintenance-requirement")]
        public double MaintenanceRequirement { get; set; }
        [JsonPropertyName("margin-equity")]
        public double MarginEquity { get; set; }
        [JsonPropertyName("margin-settle-balance")]
        public double MarginSettleBalance { get; set; }
        [JsonPropertyName("net-liquidating-value")]
        public double NetLiquidatingValue { get; set; }
    }
}
