using System.Text.Json.Serialization;
using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Library.Models.ViewModels;

public class TournamentViewModel : Tournament
{
    [JsonPropertyName("registeredTeams")]
    public List<TeamViewModel>? RegisteredTeams { get; set; }

    [JsonPropertyName("results")]
    public List<TeamScoreViewModel>? Results { get; set; } = [];
}
