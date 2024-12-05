using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountSnapShotDto : AbstractDTO
    {
        public AccountSnapShotDto(JsonDocument jsonDocument) : base(jsonDocument) { }

        [JsonPropertyName("items")]
        public List<AccountSnapShot> Items { get; set; }
    }
}
