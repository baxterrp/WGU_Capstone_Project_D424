using System.IdentityModel.Tokens.Jwt;

namespace FishingTournamentTracker.Api.Configuration;

public class TokenConfiguration
{
    public string? Subject { get; set; }
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}
