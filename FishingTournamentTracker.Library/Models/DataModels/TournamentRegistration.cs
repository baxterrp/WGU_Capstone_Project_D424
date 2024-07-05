using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class TournamentRegistration : IDatabaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("userOne")]
    public string? UserOne { get; set; }

    [JsonPropertyName("userTwo")]
    public string? UserTwo { get; set; }

    [JsonPropertyName("tournament")]
    public string? Tournament { get; set; }
}
