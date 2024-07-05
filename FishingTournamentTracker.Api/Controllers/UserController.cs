using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FishingTournamentTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        return await ExecuteControllerAction(async () => Ok(await userService.AddUser(user)));
    }

    [HttpPut]
    public async Task<IActionResult> EditUser([FromBody] User user)
    {
        return await ExecuteControllerAction(async () => Ok(await userService.EditUser(user)));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await userService.GetUserById(userId));
        });
    }

    [HttpGet]
    public async Task<IActionResult> FilterUsers([FromQuery] string? name, [FromQuery] int? grade)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await userService.FilterUsers(new UserFilter
            {
                Name = name,
                Grade = grade
            }));
        });
    }

    [HttpPost("automated")]
    public async Task<IActionResult> AutomatedUserUpload([FromBody] FileUpload upload)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await userService.BulkUserUpload(upload));
        });
    }
}
