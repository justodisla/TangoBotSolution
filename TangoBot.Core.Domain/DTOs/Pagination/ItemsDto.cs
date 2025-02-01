using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBot.App.DTOs;

namespace TangoBot.Core.Domain.DTOs.Pagination
{
    public class ItemsDto
    {
        [JsonPropertyName("pagination")]
        public PaginationDto Pagination { get; set; }

    }
}
