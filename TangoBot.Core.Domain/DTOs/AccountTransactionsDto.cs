using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBot.App.DTOs;

namespace TangoBot.Core.Domain.DTOs
{
    public class AccountTransactionsDto
    {
        [JsonPropertyName("items")]
        public List<AccountTransactionDto> Transactions { get; set; }

        [JsonPropertyName("pagination")]
        public PaginationDto Pagination { get; set; }

        [JsonPropertyName("api-version")]
        public string ApiVersion { get; set; }

        [JsonPropertyName("context")]
        public string Context { get; set; }

    }
}
