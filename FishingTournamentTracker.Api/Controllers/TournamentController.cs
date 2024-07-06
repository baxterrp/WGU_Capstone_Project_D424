using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FishingTournamentTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentController(ITournamentService tournamentService) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateTournament([FromBody] Tournament tournament)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.CreateTournament(tournament));
        });
    }

    [HttpGet]
    public async Task<IActionResult> SearchTournaments()
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.FilterTournaments());
        });
    }

    [HttpGet("{tournamentId}")]
    public async Task<IActionResult> GetTournamentById([FromRoute] string tournamentId)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.GetTournamentById(tournamentId));
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterTeam([FromBody] TournamentRegistration tournamentRegistration)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.RegisterTeam(tournamentRegistration));
        });
    }

    [HttpPost("result")]
    public async Task<IActionResult> SaveTeamResult([FromBody] TeamScoreViewModel teamScoreViewModel)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.SaveTeamScore(teamScoreViewModel));
        });
    }

    [HttpGet("result/{tournamentId}")]
    public async Task<IActionResult> GetTournamentResults([FromRoute] string tournamentId)
    {
        return await ExecuteControllerAction(async () =>
        {
            return Ok(await tournamentService.GetResultPrintout(tournamentId));
        });
    }

    [HttpGet("download/{tournamentId}")]
    public async Task<IActionResult> DownloadResults([FromRoute] string tournamentId)
    {
        return await ExecuteControllerAction(async () =>
        {
            var contents = await tournamentService.DownloadResultExcel(tournamentId);
            return File(contents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{Guid.NewGuid()}.xlsx");
        });
    }
}
