using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class TournamentScore : IDatabaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("tournamentId")]
    public string? TournamentId { get; set; }

    [JsonPropertyName("teamId")]
    public string? TeamId { get; set; }
}
