using FishingTournamentTracker.Api.Controllers;
using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FishingTournamentTracker.Api.UnitTests.Controllers;

public class AdminControllerTests : BaseTestController
{
    private readonly Mock<IAdminService> _mockAdminService;
    private readonly AdminController _adminController;

    public AdminControllerTests()
    {
        _mockAdminService = new Mock<IAdminService>();
        _adminController = new AdminController(_mockAdminService.Object);
    }

    [Fact]
    public async Task Register_WithAdminRequest_ReturnsOkObjectResult()
    {
        // Arrange
        var admin = GetTestAdmin();
        _mockAdminService.Setup(service => service.RegisterNewAdmin(admin)).ReturnsAsync(admin);

        // Act
        var result = await _adminController.Register(admin);

        // Assert
        _mockAdminService.VerifyAll();
        AssertActionResultResponse<Admin, OkObjectResult>(admin, result);
    }

    [Fact]
    public async Task Login_WithAdminRequest_ReturnsOkObjectResult()
    {
        // Arrange
        var admin = GetTestAdmin();
        _mockAdminService.Setup(service => service.Login(admin)).ReturnsAsync(admin);

        // Act
        var result = await _adminController.Login(admin);

        // Assert
        _mockAdminService.VerifyAll();
        AssertActionResultResponse<Admin, OkObjectResult>(admin, result);
    }

    private static Admin GetTestAdmin()
    {
        return new Admin
        {
            Id = Guid.NewGuid().ToString(),
            UserName = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
        };
    }
}
