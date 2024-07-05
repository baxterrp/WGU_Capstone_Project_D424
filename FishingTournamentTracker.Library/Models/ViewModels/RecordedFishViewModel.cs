using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.ViewModels;

public class RecordedFishViewModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("weight")]
    public double? Weight { get; set; }
    public bool Weighed
    {
        get
        {
            return Weight.HasValue && Weight.Value > 0;
        }
    }

    [JsonPropertyName("scoreId")]
    public string? ScoreId { get; set; }
}
