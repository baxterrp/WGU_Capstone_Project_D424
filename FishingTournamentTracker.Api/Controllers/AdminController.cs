using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace FishingTournamentTracker.Api.Controllers;

public class AdminController(IAdminService adminService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Admin admin)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await adminService.RegisterNewAdmin(admin));
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Admin admin)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await adminService.Login(admin));
        });
    }
}
