using HttpClientLib.OrderApi.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class OrderPostReport
{
    [JsonPropertyName("data")]
    public ReportData? Data { get; set; }

    [JsonPropertyName("context")]
    public string? Context { get; set; }
}

public class ReportData
{
    [JsonPropertyName("buying-power-effect")]
    public BuyingPowerEffect BuyingPowerEffect { get; set; }

    [JsonPropertyName("fee-calculation")]
    public FeeCalculation FeeCalculation { get; set; }

    [JsonPropertyName("order")]
    public Order Order { get; set; }

    [JsonPropertyName("warnings")]
    public List<Warning> Warnings { get; set; }
}

public class BuyingPowerEffect
{
    [JsonPropertyName("change-in-margin-requirement")]
    public string ChangeInMarginRequirement { get; set; }

    [JsonPropertyName("change-in-margin-requirement-effect")]
    public string ChangeInMarginRequirementEffect { get; set; }

    [JsonPropertyName("change-in-buying-power")]
    public string ChangeInBuyingPower { get; set; }

    [JsonPropertyName("change-in-buying-power-effect")]
    public string ChangeInBuyingPowerEffect { get; set; }

    [JsonPropertyName("current-buying-power")]
    public string CurrentBuyingPower { get; set; }

    [JsonPropertyName("current-buying-power-effect")]
    public string CurrentBuyingPowerEffect { get; set; }

    [JsonPropertyName("new-buying-power")]
    public string NewBuyingPower { get; set; }

    [JsonPropertyName("new-buying-power-effect")]
    public string NewBuyingPowerEffect { get; set; }

    [JsonPropertyName("isolated-order-margin-requirement")]
    public string IsolatedOrderMarginRequirement { get; set; }

    [JsonPropertyName("isolated-order-margin-requirement-effect")]
    public string IsolatedOrderMarginRequirementEffect { get; set; }

    [JsonPropertyName("is-spread")]
    public bool IsSpread { get; set; }

    [JsonPropertyName("impact")]
    public string Impact { get; set; }

    [JsonPropertyName("effect")]
    public string Effect { get; set; }
}

public class FeeCalculation
{
    [JsonPropertyName("regulatory-fees")]
    public string RegulatoryFees { get; set; }

    [JsonPropertyName("regulatory-fees-effect")]
    public string RegulatoryFeesEffect { get; set; }

    [JsonPropertyName("regulatory-fees-breakdown")]
    public List<FeeBreakdown> RegulatoryFeesBreakdown { get; set; }

    [JsonPropertyName("clearing-fees")]
    public string ClearingFees { get; set; }

    [JsonPropertyName("clearing-fees-effect")]
    public string ClearingFeesEffect { get; set; }

    [JsonPropertyName("clearing-fees-breakdown")]
    public List<FeeBreakdown> ClearingFeesBreakdown { get; set; }

    [JsonPropertyName("commission")]
    public string Commission { get; set; }

    [JsonPropertyName("commission-effect")]
    public string CommissionEffect { get; set; }

    [JsonPropertyName("commission-breakdown")]
    public List<FeeBreakdown> CommissionBreakdown { get; set; }

    [JsonPropertyName("proprietary-index-option-fees")]
    public string ProprietaryIndexOptionFees { get; set; }

    [JsonPropertyName("proprietary-index-option-fees-effect")]
    public string ProprietaryIndexOptionFeesEffect { get; set; }

    [JsonPropertyName("proprietary-fees-breakdown")]
    public List<FeeBreakdown> ProprietaryFeesBreakdown { get; set; }

    [JsonPropertyName("total-fees")]
    public string TotalFees { get; set; }

    [JsonPropertyName("total-fees-effect")]
    public string TotalFeesEffect { get; set; }
}

public class FeeBreakdown
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("effect")]
    public string Effect { get; set; }
}

public class Warning
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
