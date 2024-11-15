using System.Text.Json.Serialization;

public class AccountListResponse
{
    public AccountListData data { get; set; }
}

public class AccountListData
{
    public List<AccountItem> items { get; set; }
}

public class AccountItem
{
    public AccountInfo account { get; set; }

    [JsonPropertyName("authority-level")]
    public string AuthorityLevel { get; set; }
}

public class AccountInfo // Previously named AccountDetails
{
    [JsonPropertyName("account-number")]
    public string AccountNumber { get; set; }

    [JsonPropertyName("account-type-name")]
    public string AccountTypeName { get; set; }

    [JsonPropertyName("created-at")]
    public DateTime? CreatedAt { get; set; }

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

    public string Nickname { get; set; }

    [JsonPropertyName("opened-at")]
    public DateTime? OpenedAt { get; set; }

    [JsonPropertyName("suitable-options-level")]
    public string SuitableOptionsLevel { get; set; }
}
