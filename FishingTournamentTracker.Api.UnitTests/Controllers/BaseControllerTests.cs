using FishingTournamentTracker.Api.Controllers;
using FishingTournamentTracker.Api.Exeptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FishingTournamentTracker.Api.UnitTests.Controllers
{
    public class BaseControllerTests : BaseTestController
    {
        private readonly Mock<ITestService> _mockTestService;
        private readonly TestController _testController;

        public BaseControllerTests()
        {
            _mockTestService = new Mock<ITestService>();
            _testController = new TestController(_mockTestService.Object);
        }

        [Fact]
        public async Task ExecuteControllerAction_WhenServiceReturnsOk_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedResponse = Guid.NewGuid().ToString();
            _mockTestService.Setup(service => service.Test()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _testController.TestEndPoint();

            // Assert
            _mockTestService.VerifyAll();
            AssertActionResultResponse<string, OkObjectResult>(expectedResponse, result);
        }

        [Fact]
        public async Task ExecuteControllerAction_WhenServiceThrowsArgumentException_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var exception = new ArgumentException(Guid.NewGuid().ToString());
            _mockTestService.Setup(service => service.Test()).ThrowsAsync(exception);

            // Act
            var result = await _testController.TestEndPoint();

            // Assert
            _mockTestService.VerifyAll();
            AssertActionResultResponse<string, BadRequestObjectResult>(exception.Message, result);
        }

        [Fact]
        public async Task ExecuteControllerAction_WhenServiceThrowsUnauthorizedLoginAttemptException_ReturnsUnauthorized()
        {
            // Arrange
            _mockTestService.Setup(service => service.Test()).ThrowsAsync(new UnauthorizedLoginAttemptException());

            // Act
            var result = await _testController.TestEndPoint();

            // Assert
            _mockTestService.VerifyAll();
            result.Should().BeAssignableTo<UnauthorizedResult>();
        }

        [Fact]
        public async Task ExecuteControllerAction_WhenServiceThrowsGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            _mockTestService.Setup(service => service.Test()).ThrowsAsync(new Exception());

            // Act
            var result = await _testController.TestEndPoint();

            // Assert
            _mockTestService.VerifyAll();

            var objectResult = result.Should().BeAssignableTo<StatusCodeResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
        }

        public interface ITestService
        {
            Task<string> Test();
        }

        public class TestController(ITestService testService) : BaseApiController
        {
            public async Task<IActionResult> TestEndPoint()
            {
                return await ExecuteControllerAction(async () =>
                {
                    return Ok(await testService.Test());
                });
            }
        }
    }
}
