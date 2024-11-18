using System.Text.Json.Serialization;

public class CustomerInfoResponse
{
    public CustomerInfo data { get; set; }
}

public class CustomerInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("first-name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last-name")]
    public string LastName { get; set; }

    [JsonPropertyName("mobile-phone-number")]
    public string MobilePhoneNumber { get; set; }

    [JsonPropertyName("birth-date")]
    public string BirthDate { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("tax-number-type")]
    public string TaxNumberType { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("mailing-address")]
    public Address MailingAddress { get; set; }

    [JsonPropertyName("is-foreign")]
    public bool IsForeign { get; set; }

    [JsonPropertyName("usa-citizenship-type")]
    public string UsaCitizenshipType { get; set; }

    [JsonPropertyName("agreed-to-margining")]
    public bool AgreedToMargining { get; set; }

    [JsonPropertyName("subject-to-tax-withholding")]
    public bool SubjectToTaxWithholding { get; set; }

    [JsonPropertyName("permitted-account-types")]
    public AccountType[] PermittedAccountTypes { get; set; }

    [JsonPropertyName("created-at")]
    public string CreatedAt { get; set; }
}

public class Address
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("is-domestic")]
    public bool IsDomestic { get; set; }

    [JsonPropertyName("is-foreign")]
    public bool IsForeign { get; set; }

    [JsonPropertyName("postal-code")]
    public string PostalCode { get; set; }

    [JsonPropertyName("state-region")]
    public string StateRegion { get; set; }

    [JsonPropertyName("street-one")]
    public string StreetOne { get; set; }
}

public class AccountType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("is_tax_advantaged")]
    public bool IsTaxAdvantaged { get; set; }

    [JsonPropertyName("has_multiple_owners")]
    public bool HasMultipleOwners { get; set; }

    [JsonPropertyName("is_publicly_available")]
    public bool IsPubliclyAvailable { get; set; }

    [JsonPropertyName("margin_types")]
    public MarginType[] MarginTypes { get; set; }
}

public class MarginType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("is_margin")]
    public bool IsMargin { get; set; }
}
