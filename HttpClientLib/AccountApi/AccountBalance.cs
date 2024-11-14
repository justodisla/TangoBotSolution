using System.Text.Json.Serialization;

public class AccountBalanceResponse
{
    public AccountBalance data { get; set; }
}

public class AccountBalance
{
    [JsonPropertyName("account-number")]
    public string AccountNumber { get; set; }

    [JsonPropertyName("available-trading-funds")]
    public string AvailableTradingFunds { get; set; }

    [JsonPropertyName("cash-available-to-withdraw")]
    public string CashAvailableToWithdraw { get; set; }

    [JsonPropertyName("cash-balance")]
    public string CashBalance { get; set; }

    [JsonPropertyName("cash-settle-balance")]
    public string CashSettleBalance { get; set; }

    [JsonPropertyName("closed-loop-available-balance")]
    public string ClosedLoopAvailableBalance { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("day-trading-buying-power")]
    public string DayTradingBuyingPower { get; set; }

    [JsonPropertyName("equity-buying-power")]
    public string EquityBuyingPower { get; set; }

    [JsonPropertyName("net-liquidating-value")]
    public string NetLiquidatingValue { get; set; }

    [JsonPropertyName("maintenance-excess")]
    public string MaintenanceExcess { get; set; }

    [JsonPropertyName("updated-at")]
    public string UpdatedAt { get; set; }

    // Add any other fields with their respective [JsonPropertyName] attributes as needed
}
