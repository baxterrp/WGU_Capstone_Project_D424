using FishingTournamentTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FishingTournamentTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController(ITokenService tokenService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetToken()
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(tokenService.GetToken());
        });
    }
}
