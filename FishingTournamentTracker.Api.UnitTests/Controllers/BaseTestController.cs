using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FishingTournamentTracker.Api.UnitTests.Controllers;

public abstract class BaseTestController
{
    protected static void AssertActionResultResponse<TResultType, TActionResultType>(TResultType expectedResult, IActionResult actionResult)
        where TActionResultType : ObjectResult
    {
        var okObjectResult = actionResult.Should().BeAssignableTo<TActionResultType>().Subject;
        var actualResponseBody = okObjectResult.Value.Should().BeAssignableTo<TResultType>().Subject;
        actualResponseBody.Should().BeEquivalentTo(expectedResult);
    }
}
