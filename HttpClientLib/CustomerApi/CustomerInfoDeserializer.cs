using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientLib.CustomerApi
{
    public class CustomerInfoDeserializer
    {
        public Customer DeserializeCustomerInfo(string responseBody)
        {
            var customerData = JsonSerializer.Deserialize<CustomerData>(responseBody);
            return customerData?.Data;
        }

        private class CustomerData
        {
            [JsonPropertyName("data")]
            public Customer Data { get; set; }
        }
    }

    public class Customer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("first-name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last-name")]
        public string LastName { get; set; }

        [JsonPropertyName("address")]
        public Address CAddress { get; set; }

        [JsonPropertyName("mailing-address")]
        public MailingAddress CMailingAddress { get; set; }

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

        [JsonPropertyName("permitted-account-types")]
        public List<AccountType> PermittedAccountTypes { get; set; }

        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"Customer: {FirstName} {LastName}, Email: {Email}, Phone: {MobilePhoneNumber}";
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

        public class MailingAddress
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
            public List<MarginType> MarginTypes { get; set; }
        }

        public class MarginType
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("is_margin")]
            public bool IsMargin { get; set; }
        }
    }
}
