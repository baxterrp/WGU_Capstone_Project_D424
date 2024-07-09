using FishingTournamentTracker.Api.Exeptions;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FluentAssertions;
using Moq;

namespace FishingTournamentTracker.Api.UnitTests.Services;

public class AdminServiceUnitTests
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly AdminService _adminService;

    public AdminServiceUnitTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _adminService = new AdminService(_mockAdminRepository.Object);
    }

    [Fact]
    public async Task Login_WhenAdminIsNull_ThrowsArgumentNullException()
    {
        // Arrange/Act/Assert
        var controllerAction = async () => await _adminService.Login(null!);
        await controllerAction.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'admin')")
            .WithParameterName("admin");
    }

    [Fact]
    public async Task Login_WhenAdminUserNameIsNull_ThrowsArgumentException()
    {
        // Arrange/Act/Assert
        var controllerAction = async () => await _adminService.Login(new Admin());
        await controllerAction.Should().ThrowAsync<ArgumentException>()
            .WithParameterName(nameof(Admin.UserName));
    }

    [Fact]
    public async Task Login_WhenRepositoryCallReturnsNull_ThrowsUnauthorizedLoginAttemptException()
    {
        // Arrange
        var admin = new Admin
        {
            UserName = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
        };

        _mockAdminRepository.Setup(repo => repo.Login(admin)).ReturnsAsync((Admin?)null);

        // Act/Assert
        var controllerAction = async () => await _adminService.Login(admin);
        await controllerAction.Should().ThrowAsync<UnauthorizedLoginAttemptException>();
    }
}
