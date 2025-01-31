using System;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountTransactionDto
    {
        public AccountTransactionDto() { }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("amount")]
        public string AmountRaw { get; set; }
        [JsonIgnore]
        public double Amount => double.TryParse(AmountRaw, out double value) ? value : 0;

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("net-amount")]
        public string NetAmountRaw { get; set; }
        [JsonIgnore]
        public double NetAmount => double.TryParse(NetAmountRaw, out double value) ? value : 0;

        [JsonPropertyName("running-balance")]
        public string RunningBalanceRaw { get; set; }
        [JsonIgnore]
        public double RunningBalance => double.TryParse(RunningBalanceRaw, out double value) ? value : 0;

        [JsonPropertyName("commission")]
        public string CommissionRaw { get; set; }
        [JsonIgnore]
        public double Commission => double.TryParse(CommissionRaw, out double value) ? value : 0;

        [JsonPropertyName("fees")]
        public string FeesRaw { get; set; }
        [JsonIgnore]
        public double Fees => double.TryParse(FeesRaw, out double value) ? value : 0;

        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated-at")]
        public DateTime UpdatedAt { get; set; }
    }
}
