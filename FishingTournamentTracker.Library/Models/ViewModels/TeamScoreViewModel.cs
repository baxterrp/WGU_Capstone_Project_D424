using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.ViewModels;

public class TeamScoreViewModel
{
    [JsonPropertyName("id")]
    public string? Id {  get; set; }

    [JsonPropertyName("tournamentId")]
    public string? TournamentId { get; set; }

    [JsonPropertyName("teamId")]
    public string? TeamId { get; set; }

    [JsonPropertyName("name")]
    public string? Name {  get; set; }

    [JsonPropertyName("fish")]
    public List<RecordedFishViewModel> Fish { get; set; } = [];
}
