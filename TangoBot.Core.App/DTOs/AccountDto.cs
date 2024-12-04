using System.Text.Json;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountDto : AbstractDTO
    {
        public AccountDto(JsonDocument jsonDocument) : base(jsonDocument) { }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; }
        [JsonPropertyName("account-type-name")]
        public string AccountTypeName { get; set; }
        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }
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
        // Add other properties as needed
    }
}
