using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class RecordedFish : IDatabaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("weight")]
    public double? Weight { get; set; }

    [JsonPropertyName("scoreId")]
    public string? ScoreId {  get; set; }
}
