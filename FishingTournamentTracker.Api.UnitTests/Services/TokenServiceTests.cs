using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace FishingTournamentTracker.Api.UnitTests.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly IOptions<TokenConfiguration> _tokenConfiguration;

    public TokenServiceTests()
    {
        _tokenConfiguration = Options.Create(new TokenConfiguration
        {
            Issuer = Guid.NewGuid().ToString(),
            Audience = Guid.NewGuid().ToString(),
            Key = Guid.NewGuid().ToString(),
            Subject = Guid.NewGuid().ToString()
        });

        _tokenService = new TokenService(_tokenConfiguration);
    }

    [Fact]
    public void GetToken_ReturnsNonEmptyString()
    {
        // Act
        var token = _tokenService.GetToken();

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
    }
}
