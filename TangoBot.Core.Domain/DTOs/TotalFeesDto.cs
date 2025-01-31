using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TangoBot.Core.Domain.DTOs
{
    public class TotalFeesDto
    {
        public TotalFeesDto()
        {
            
        }
        [JsonPropertyName("total-fees")]
        public string TotalFeesRaw { get; set; }
        
        [JsonIgnore]
        public double TotalFees => double.TryParse(TotalFeesRaw, out double value) ? value : 0;

        [JsonPropertyName("total-fees-effect")]
        public string TotalFeesEffect { get; set; }
    }
}
