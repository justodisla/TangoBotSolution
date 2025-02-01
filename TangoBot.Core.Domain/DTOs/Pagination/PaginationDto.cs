using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class PaginationDto
    {
        public PaginationDto() { }

        [JsonPropertyName("per-page")]
        public int PerPage { get; set; }

        [JsonPropertyName("page-offset")]
        public int PageOffset { get; set; }

        [JsonPropertyName("item-offset")]
        public int ItemOffset { get; set; }

        [JsonPropertyName("total-items")]
        public int TotalItems { get; set; }

        [JsonPropertyName("total-pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("current-item-count")]
        public int CurrentItemCount { get; set; }

        [JsonPropertyName("previous-link")]
        public string PreviousLink { get; set; }

        [JsonPropertyName("next-link")]
        public string NextLink { get; set; }

        [JsonPropertyName("paging-link-template")]
        public string PagingLinkTemplate { get; set; }
    }
}
