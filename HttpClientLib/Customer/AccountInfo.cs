using System.Text.Json.Serialization;

public class AccountListResponse
{
    public AccountList data { get; set; }
}

public class AccountList
{
    public AccountInfo[] items { get; set; }
}

public class AccountInfo
{
    [JsonPropertyName("account-number")]
    public string AccountNumber { get; set; }

    [JsonPropertyName("account-type-name")]
    public string AccountTypeName { get; set; }

    [JsonPropertyName("created-at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("day-trader-status")]
    public bool DayTraderStatus { get; set; }

    [JsonPropertyName("investment-objective")]
    public string InvestmentObjective { get; set; }

    [JsonPropertyName("is-closed")]
    public bool IsClosed { get; set; }

    [JsonPropertyName("is-firm-error")]
    public bool IsFirmError { get; set; }

    [JsonPropertyName("is-firm-proprietary")]
    public bool IsFirmProprietary { get; set; }

    [JsonPropertyName("is-foreign")]
    public bool IsForeign { get; set; }

    [JsonPropertyName("is-futures-approved")]
    public bool IsFuturesApproved { get; set; }

    [JsonPropertyName("is-test-drive")]
    public bool IsTestDrive { get; set; }

    [JsonPropertyName("margin-or-cash")]
    public string MarginOrCash { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("opened-at")]
    public string OpenedAt { get; set; }

    [JsonPropertyName("suitable-options-level")]
    public string SuitableOptionsLevel { get; set; }

    [JsonPropertyName("authority-level")]
    public string AuthorityLevel { get; set; }
}

public class AccountDetailsResponse
{
    public AccountInfo data { get; set; }
}
