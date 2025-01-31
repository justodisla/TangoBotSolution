using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBot.App.DTOs;

namespace TangoBot.Core.Domain.DTOs
{
    public class InstrumentsDto
    {
        public InstrumentsDto() { }

        [JsonPropertyName("items")]
        public List<InstrumentDto> Items { get; set; }
    }
}
