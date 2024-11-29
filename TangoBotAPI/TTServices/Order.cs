using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TangoBot.API.TTServices
{
    public class Order
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("account-number")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("cancellable")]
        public bool Cancellable { get; set; }

        [JsonPropertyName("editable")]
        public bool Editable { get; set; }

        [JsonPropertyName("edited")]
        public bool Edited { get; set; }

        [JsonPropertyName("ext-client-order-id")]
        public string ExtClientOrderId { get; set; } = string.Empty;

        [JsonPropertyName("ext-exchange-order-number")]
        public string ExtExchangeOrderNumber { get; set; } = string.Empty;

        [JsonPropertyName("ext-global-order-number")]
        public int ExtGlobalOrderNumber { get; set; }

        [JsonPropertyName("global-request-id")]
        public string GlobalRequestId { get; set; } = string.Empty;

        [JsonPropertyName("order-type")]
        public string OrderType { get; set; } = string.Empty;

        [JsonPropertyName("price-effect")]
        public string PriceEffect { get; set; } = string.Empty;

        [JsonPropertyName("received-at")]
        public DateTime ReceivedAt { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("terminal-at")]
        public DateTime TerminalAt { get; set; }

        [JsonPropertyName("time-in-force")]
        public string TimeInForce { get; set; } = string.Empty;

        [JsonPropertyName("underlying-instrument-type")]
        public string UnderlyingInstrumentType { get; set; } = string.Empty;

        [JsonPropertyName("underlying-symbol")]
        public string UnderlyingSymbol { get; set; } = string.Empty;

        [JsonPropertyName("updated-at")]
        public long UpdatedAt { get; set; }

        [JsonPropertyName("legs")]
        public List<Leg> Legs { get; set; } = new();
    }

    public class Leg
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("instrument-type")]
        public string InstrumentType { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("remaining-quantity")]
        public int RemainingQuantity { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("fills")]
        public List<Fill> Fills { get; set; } = new();
    }

    public class Fill
    {
        [JsonPropertyName("destination-venue")]
        public string DestinationVenue { get; set; } = string.Empty;

        [JsonPropertyName("ext-exec-id")]
        public string ExtExecId { get; set; } = string.Empty;

        [JsonPropertyName("ext-group-fill-id")]
        public string ExtGroupFillId { get; set; } = string.Empty;

        [JsonPropertyName("fill-id")]
        public string FillId { get; set; } = string.Empty;

        [JsonPropertyName("fill-price")]
        public string FillPrice { get; set; } = string.Empty;

        [JsonPropertyName("filled-at")]
        public DateTime FilledAt { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
