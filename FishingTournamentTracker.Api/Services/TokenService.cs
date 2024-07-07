using FishingTournamentTracker.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FishingTournamentTracker.Api.Services;

public class TokenService(IOptions<TokenConfiguration> tokenConfiguration) : ITokenService
{
    public string GetToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Value.Key!));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            tokenConfiguration.Value.Issuer,
            tokenConfiguration.Value.Audience,
            [
                new Claim(JwtRegisteredClaimNames.Sub, tokenConfiguration.Value.Subject!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
            ], 
            expires: DateTime.UtcNow.AddMinutes(10), 
            signingCredentials: signIn);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
