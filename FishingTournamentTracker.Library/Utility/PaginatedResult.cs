using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Utility;

public class PaginatedResult<TEntity>
{
    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int? PageSize { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable<TEntity>? Data { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }
}
