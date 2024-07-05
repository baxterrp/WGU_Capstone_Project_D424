using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.ViewModels
{
    public class TournamentResultPrintout
    {
        [JsonPropertyName("place")]
        public int? Place {  get; set; }

        [JsonPropertyName("team")]
        public string? Team {  get; set; }

        [JsonPropertyName("fish")]
        public string? Fish {  get; set; }

        [JsonPropertyName("biggestFish")]
        public double? BiggestFish { get; set; }

        [JsonPropertyName("totalWeight")]
        public double? TotalWeight { get; set; }

        [JsonPropertyName("points")]
        public int? Points { get; set; }
    }
}
