using System;
using System.Text.Json.Serialization;

namespace HttpClientLib.AccountApi
{
    public class Account
    {
        private bool isClosed;
        private bool isFirmError;
        private bool isFirmProprietary;
        private bool isForeign;
        private bool isFuturesApproved;
        private bool isTestDrive;
        private string? marginOrCash;
        private string? nickname;
        private string? suitableOptionsLevel;
        private string? authorityLevel;

        [JsonPropertyName("account-number")]
        public string? AccountNumber { get; set; }

        [JsonPropertyName("account-type-name")]
        public string? AccountTypeName { get; set; }

        [JsonPropertyName("created-at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("day-trader-status")]
        public bool DayTraderStatus { get; set; }

        [JsonPropertyName("investment-objective")]
        public string? InvestmentObjective { get; set; }

        [JsonPropertyName("is-closed")]
        public bool IsClosed { get => isClosed; set => isClosed = value; }

        [JsonPropertyName("is-firm-error")]
        public bool IsFirmError { get => isFirmError; set => isFirmError = value; }

        [JsonPropertyName("is-firm-proprietary")]
        public bool IsFirmProprietary { get => isFirmProprietary; set => isFirmProprietary = value; }

        [JsonPropertyName("is-foreign")]
        public bool IsForeign { get => isForeign; set => isForeign = value; }

        [JsonPropertyName("is-futures-approved")]
        public bool IsFuturesApproved { get => isFuturesApproved; set => isFuturesApproved = value; }

        [JsonPropertyName("is-test-drive")]
        public bool IsTestDrive { get => isTestDrive; set => isTestDrive = value; }

        [JsonPropertyName("margin-or-cash")]
        public string MarginOrCash { get => marginOrCash; set => marginOrCash = value; }

        [JsonPropertyName("nickname")]
        public string Nickname { get => nickname; set => nickname = value; }

        [JsonPropertyName("opened-at")]
        public DateTime OpenedAt { get; set; }

        [JsonPropertyName("suitable-options-level")]
        public string SuitableOptionsLevel { get => suitableOptionsLevel; set => suitableOptionsLevel = value; }

        [JsonPropertyName("authority-level")]
        public string AuthorityLevel { get => authorityLevel; set => authorityLevel = value; }
    }
}
