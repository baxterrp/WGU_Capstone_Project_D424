using FishingTournamentTracker.Api.Controllers;
using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FishingTournamentTracker.Api.UnitTests.Controllers;

public class UserControllerTests : BaseTestController
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _userController = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task DeleteUser_ReturnsOkObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var isDeleted = true;

        _mockUserService.Setup(service => service.Delete(userId)).ReturnsAsync(isDeleted);

        // Act
        var result = await _userController.DeleteUser(userId);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<bool, OkObjectResult>(isDeleted, result);
    }

    [Fact]
    public async Task AddUser_ReturnsOkObjectResult()
    {
        // Arrange
        var user = GetUser();
        _mockUserService.Setup(service => service.AddUser(user)).ReturnsAsync(user);

        // Act
        var result = await _userController.AddUser(user);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<User, OkObjectResult>(user, result);
    }

    [Fact]
    public async Task EditUser_ReturnsOkObjectResult()
    {
        // Arrange
        var user = GetUser();
        _mockUserService.Setup(service => service.EditUser(user)).ReturnsAsync(user);

        // Act
        var result = await _userController.EditUser(user);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<User, OkObjectResult>(user, result);
    }

    [Fact]
    public async Task GetUserById_ReturnsOkObjectResult()
    {
        // Arrange
        var user = GetUser();
        _mockUserService.Setup(service => service.GetUserById(user.Id!)).ReturnsAsync(user);

        // Act
        var result = await _userController.GetUserById(user.Id!);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<User, OkObjectResult>(user, result);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkObjectResult()
    {
        // Arrange
        var users = new List<User>
        {
            GetUser(),
            GetUser(),
            GetUser(),
            GetUser()
        };

        _mockUserService.Setup(service => service.GetAllUsers(It.IsAny<UserFilter>())).ReturnsAsync(users);

        // Act
        var result = await _userController.GetAllUsers();

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<List<User>, OkObjectResult>(users, result);
    }

    [Fact]
    public async Task FilterUsers_ReturnsOkObjectResult()
    {
        // Arrange
        var name = Guid.NewGuid().ToString();
        var grade = 10;
        var page = 1;
        var size = 10;

        var paginatedResult = new PaginatedResult<User>
        {
            Data =
            [
                GetUser(),
                GetUser(),
                GetUser(),
                GetUser()
            ]
        };

        _mockUserService.Setup(service => service.FilterUsers(
            It.Is<UserFilter>(
                filter => 
                    filter.Name == name && 
                    filter.Grade == grade && 
                    filter.Page == page && 
                    filter.PageSize == size)))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _userController.FilterUsers(name, grade, page, size);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<PaginatedResult<User>, OkObjectResult>(paginatedResult, result);
    }

    [Fact]
    public async Task AutomatedUserUpload_ReturnsOkObjectResult()
    {
        // Arrange
        var users = new List<User>
        {
            GetUser(),
            GetUser(),
            GetUser(),
            GetUser()
        };

        var upload = new FileUpload
        {
            Contents = Guid.NewGuid().ToString()
        };

        _mockUserService.Setup(service => service.BulkUserUpload(upload)).ReturnsAsync(users);

        // Act
        var result = await _userController.AutomatedUserUpload(upload);

        // Assert
        _mockUserService.VerifyAll();
        AssertActionResultResponse<List<User>, OkObjectResult>(users, result);
    }

    private static User GetUser()
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Birthday = DateTime.Now,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Grade = 10
        };
    }
}
