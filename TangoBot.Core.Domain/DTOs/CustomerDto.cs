using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class CustomerDto 
    {
        public CustomerDto() { }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("first-name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last-name")]
        public string LastName { get; set; }

        [JsonPropertyName("address")]
        public AddressDto Address { get; set; }

        [JsonPropertyName("mailing-address")]
        public AddressDto MailingAddress { get; set; }

        [JsonPropertyName("is-foreign")]
        public bool IsForeign { get; set; }

        [JsonPropertyName("usa-citizenship-type")]
        public string UsaCitizenshipType { get; set; }

        [JsonPropertyName("mobile-phone-number")]
        public string MobilePhoneNumber { get; set; }

        [JsonPropertyName("birth-date")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("tax-number-type")]
        public string TaxNumberType { get; set; }

        [JsonPropertyName("agreed-to-margining")]
        public bool AgreedToMargining { get; set; }

        [JsonPropertyName("subject-to-tax-withholding")]
        public bool SubjectToTaxWithholding { get; set; }

        [JsonPropertyName("has-industry-affiliation")]
        public bool HasIndustryAffiliation { get; set; }

        [JsonPropertyName("has-listed-affiliation")]
        public bool HasListedAffiliation { get; set; }

        [JsonPropertyName("has-political-affiliation")]
        public bool HasPoliticalAffiliation { get; set; }

        [JsonPropertyName("has-delayed-quotes")]
        public bool HasDelayedQuotes { get; set; }

        [JsonPropertyName("has-pending-or-approved-application")]
        public bool HasPendingOrApprovedApplication { get; set; }

        [JsonPropertyName("is-professional")]
        public bool IsProfessional { get; set; }

        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }
    }
}

