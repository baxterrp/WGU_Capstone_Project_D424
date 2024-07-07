using FishingTournamentTracker.Api.Controllers;
using FishingTournamentTracker.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FishingTournamentTracker.Api.UnitTests.Controllers;

public class TokenControllerTests
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly TokenController _tokenController;

    public TokenControllerTests()
    {
        _mockTokenService = new Mock<ITokenService>();
        _tokenController = new TokenController(_mockTokenService.Object);
    }

    [Fact]
    public async Task GetToken_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        var expectedTokenResponse = Guid.NewGuid().ToString();
        _mockTokenService.Setup(service => service.GetToken()).Returns(expectedTokenResponse);

        // Act
        var result = await _tokenController.GetToken();

        // Assert
        var okObjectResult = result.Should().BeAssignableTo<OkObjectResult>().Subject;
        var actualResponseBody = okObjectResult.Value.Should().BeAssignableTo<string>().Subject;
        actualResponseBody.Should().BeEquivalentTo(expectedTokenResponse);
    }
}
