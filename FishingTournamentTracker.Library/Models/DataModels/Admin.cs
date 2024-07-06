using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class Admin : IDatabaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName {  get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}
