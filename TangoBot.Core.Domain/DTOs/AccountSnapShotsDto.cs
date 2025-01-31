using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class AccountSnapShotsDto
    {
        public AccountSnapShotsDto()
        {
            
        }

        [JsonPropertyName("items")]
        public List<AccountSnapShotDto> Items { get; set; }
    }
}
