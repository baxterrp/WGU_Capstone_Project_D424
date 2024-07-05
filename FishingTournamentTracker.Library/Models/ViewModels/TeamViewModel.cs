using System.Text.Json.Serialization;
using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Library.Models.ViewModels
{
    public class TeamViewModel
    {
        [JsonPropertyName("id")]
        public string? Id {  get; set; }

        [JsonPropertyName("userOne")]
        public User? UserOne { get; set; }

        [JsonPropertyName("userTwo")]
        public User? UserTwo { get; set; }
    }
}
