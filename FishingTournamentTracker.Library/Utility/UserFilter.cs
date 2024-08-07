﻿using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Utility;

public class UserFilter
{
    [JsonPropertyName("name")]
    public string? Name {  get; set; }

    [JsonPropertyName("grade")]
    public int? Grade {  get; set; }

    [JsonPropertyName("page")]
    public int? Page {  get; set; }

    [JsonPropertyName("pageSize")]
    public int? PageSize { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }
}
