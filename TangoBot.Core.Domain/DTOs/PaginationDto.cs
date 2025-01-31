using System.Text.Json.Serialization;

namespace TangoBot.App.DTOs
{
    public class PaginationDto
    {
        public PaginationDto() { }

        [JsonPropertyName("per-page")]
        public string PerPageRaw { get; set; }

        [JsonIgnore]
        public int PerPage => int.TryParse(PerPageRaw, out int value) ? value : 0;

        [JsonPropertyName("page-offset")]
        public string PageOffsetRaw { get; set; }

        [JsonIgnore]
        public int PageOffset => int.TryParse(PageOffsetRaw, out int value) ? value : 0;

        [JsonPropertyName("item-offset")]
        public string ItemOffsetRaw { get; set; }

        [JsonIgnore]
        public int ItemOffset => int.TryParse(ItemOffsetRaw, out int value) ? value : 0;

        [JsonPropertyName("total-items")]
        public string TotalItemsRaw { get; set; }

        [JsonIgnore]
        public int TotalItems => int.TryParse(TotalItemsRaw, out int value) ? value : 0;

        [JsonPropertyName("total-pages")]
        public string TotalPagesRaw { get; set; }

        [JsonIgnore]
        public int TotalPages => int.TryParse(TotalPagesRaw, out int value) ? value : 0;

        [JsonPropertyName("current-item-count")]
        public string CurrentItemCountRaw { get; set; }

        [JsonIgnore]
        public int CurrentItemCount => int.TryParse(CurrentItemCountRaw, out int value) ? value : 0;

        [JsonPropertyName("previous-link")]
        public string? PreviousLink { get; set; }

        [JsonPropertyName("next-link")]
        public string? NextLink { get; set; }

        [JsonPropertyName("paging-link-template")]
        public string? PagingLinkTemplate { get; set; }
    }
}
