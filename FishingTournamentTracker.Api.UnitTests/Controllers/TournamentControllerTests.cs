using FishingTournamentTracker.Api.Controllers;
using FishingTournamentTracker.Api.Extensions;
using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net.NetworkInformation;
using System.Text;

namespace FishingTournamentTracker.Api.UnitTests.Controllers;

public class TournamentControllerTests : BaseTestController
{
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly TournamentController _tournamentController;

    public TournamentControllerTests()
    {
        _mockTournamentService = new Mock<ITournamentService>();
        _tournamentController = new TournamentController(_mockTournamentService.Object);
    }

    [Fact]
    public async Task CreateTournament_WithValidRequest_ReturnsOkObjectResultWithTournament()
    {
        // Arrange
        var tournament = GetTournament();
        _mockTournamentService.Setup(service => service.CreateTournament(tournament)).ReturnsAsync(tournament);

        // Act
        var result = await _tournamentController.CreateTournament(tournament);

        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<Tournament, OkObjectResult>(tournament, result);
    }

    [Fact]
    public async Task SearchTournaments_ReturnsOkObjectResult()
    {
        // Arrange
        var tournaments = new List<Tournament>
        {
            GetTournament(),
            GetTournament(),
            GetTournament(),
            GetTournament(),
        };

        _mockTournamentService.Setup(service => service.FilterTournaments()).ReturnsAsync(tournaments);

        // Act
        var result = await _tournamentController.SearchTournaments();

        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<List<Tournament>, OkObjectResult>(tournaments, result);
    }

    [Fact]
    public async Task GetTournamentById_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedTournament = GetTournamentViewModel();
        _mockTournamentService.Setup(service => service.GetTournamentById(expectedTournament.Id!)).ReturnsAsync(expectedTournament);

        // Act
        var result = await _tournamentController.GetTournamentById(expectedTournament.Id!);

        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<TournamentViewModel, OkObjectResult>(expectedTournament, result);
    }

    [Fact]
    public async Task RegisterTeam_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedRegistation = new TournamentRegistration
        {
            Id = Guid.NewGuid().ToString(),
            Tournament = Guid.NewGuid().ToString(),
            UserOne = Guid.NewGuid().ToString(),
            UserTwo = Guid.NewGuid().ToString(),
        };

        _mockTournamentService.Setup(service => service.RegisterTeam(expectedRegistation)).ReturnsAsync(expectedRegistation);

        // Act
        var result = await _tournamentController.RegisterTeam(expectedRegistation);
        
        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<TournamentRegistration, OkObjectResult>(expectedRegistation, result);
    }

    [Fact]
    public async Task SaveTeamResult_ReturnsOkObjectResult()
    {
        // Arrange
        var teamScore = new TeamScoreViewModel
        {
            Id = Guid.NewGuid().ToString(),
            TeamId = Guid.NewGuid().ToString(),
            TournamentId = Guid.NewGuid().ToString(),
            Fish =
            [
                new()
                {
                    Id = Guid.NewGuid().ToString()
                }
            ],
            Name = Guid.NewGuid().ToString()
        };

        _mockTournamentService.Setup(service => service.SaveTeamScore(teamScore)).ReturnsAsync(teamScore);

        // Act
        var result = await _tournamentController.SaveTeamResult(teamScore);

        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<TeamScoreViewModel, OkObjectResult>(teamScore, result);
    }

    [Fact]
    public async Task GetTournamentResults_ReturnsOkObjectResult()
    {
        // Arrange
        var tournamentId = Guid.NewGuid().ToString();
        var expectedResults = new List<TournamentResultPrintout>
        {
            new() 
            {
                BiggestFish = 10,
                Fish = "5/5",
                Place = 1,
                Points = 10,
                Team = Guid.NewGuid().ToString(),
                TotalWeight = 15
            }
        };

        _mockTournamentService.Setup(service => service.GetResultPrintout(tournamentId)).ReturnsAsync(expectedResults);

        // Act
        var result = await _tournamentController.GetTournamentResults(tournamentId);
        
        // Assert
        _mockTournamentService.VerifyAll();
        AssertActionResultResponse<List<TournamentResultPrintout>, OkObjectResult>(expectedResults, result);
    }

    [Fact]
    public async Task DownloadResults_ReturnsFileResultWithContents()
    {
        // Arrange
        var tournamentId = Guid.NewGuid().ToString();
        var expectedFileContents = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());

        _mockTournamentService.Setup(service => service.DownloadResultExcel(tournamentId)).ReturnsAsync(expectedFileContents);

        // Act
        var result = await _tournamentController.DownloadResults(tournamentId);

        // Assert
        _mockTournamentService.VerifyAll();

        var fileResult = result.Should().BeAssignableTo<FileContentResult>().Subject;
        fileResult.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        fileResult.FileContents.Should().BeEquivalentTo(expectedFileContents);
    }

    private static Tournament GetTournament()
    {
        return new Tournament
        {
            Id = Guid.NewGuid().ToString(),
            Lake = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
        };
    }

    private static TournamentViewModel GetTournamentViewModel()
    {
        return new TournamentViewModel
        {
            Id = Guid.NewGuid().ToString(),
            Lake = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
            RegisteredTeams =
            [
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserOne = new User
                    {
                        Id = Guid.NewGuid().ToString()
                    },
                    UserTwo = new User
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            ],
            Results =
            [
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                }
            ]
        };
    }
}
