using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class Tournament : IDatabaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("lake")]
    public string? Lake { get; set; }
}
