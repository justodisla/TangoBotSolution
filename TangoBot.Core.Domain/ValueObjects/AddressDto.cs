using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AddressDto
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
}

