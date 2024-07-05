using System.Text.Json.Serialization;

namespace FishingTournamentTracker.Library.Models.DataModels;

public class User : IDatabaseEntity
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("grade")]
    public int? Grade { get; set; }

    [JsonPropertyName("birthday")]
    public DateTime? Birthday { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
