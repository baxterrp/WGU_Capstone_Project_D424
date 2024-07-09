using FishingTournamentTracker.Api.Extensions;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FluentAssertions;
using Moq;
using System.Text;
using Xunit.Sdk;

namespace FishingTournamentTracker.Api.UnitTests.Services;

public class TournamentServiceTests
{
    private readonly Mock<ITournamentRepository> _mockTournamentRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IFileParser> _mockFileParser;
    private readonly TournamentService _tournamentService;

    public TournamentServiceTests()
    {
        _mockTournamentRepository = new Mock<ITournamentRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockFileParser = new Mock<IFileParser>();
        _tournamentService = new TournamentService(_mockTournamentRepository.Object, _mockUserRepository.Object, _mockFileParser.Object);
    }

    [Fact]
    public async Task DeleteTournament_WithInvalidTournamentId_ThrowsArgumentException()
    {
        // Arrange
        var tournamentId = string.Empty;

        // Act
        var serviceAction = async () => await _tournamentService.DeleteTournament(tournamentId);

        // Assert
        await serviceAction.Should().ThrowAsync<ArgumentException>().WithParameterName(nameof(tournamentId));
    }

    [Fact]
    public async Task DeleteTournament_WithValidTournamentId_ReturnsTrue()
    {
        // Arrange
        var tournamentId = Guid.NewGuid().ToString();
        _mockTournamentRepository.Setup(repo => repo.DeleteTournament(tournamentId)).ReturnsAsync(true);

        // Act
        var result = await _tournamentService.DeleteTournament(tournamentId);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DownloadResultExcel_WithInvalidTournamentId_ThrowsArgumentException()
    {
        // Arrange
        var tournamentId = string.Empty;

        // Act
        var serviceAction = async () => await _tournamentService.DownloadResultExcel(tournamentId);

        // Assert
        await serviceAction.Should().ThrowAsync<ArgumentException>().WithParameterName(nameof(tournamentId));
    }

    [Fact]
    public async Task DownloadResultExcel_WithValidTournamentId_ReturnsNonEmptyByteArray()
    {
        // Arrange
        var expectedBytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
        var tournament = GetTournament();
        SetupTournament(tournament);

        _mockFileParser.Setup(repo => repo.GenerateExcel(It.IsAny<IEnumerable<TournamentResultPrintout>>())).Returns(expectedBytes);

        // Act
        var result = await _tournamentService.DownloadResultExcel(tournament.Id!);

        // Assert
        _mockFileParser.VerifyAll();
        result.Should().BeEquivalentTo(expectedBytes);
    }

    [Theory]
    [MemberData(nameof(CreateTournamentTestData))]
    public async Task CreateTournament_WithNullTournament_ThrowsArgumentException(Tournament? tournament, string parameterName)
    {
        // Act
        var serviceCall = async () => await _tournamentService.CreateTournament(tournament!);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName(parameterName);
    }

    [Fact]
    public async Task CreateTournament_WithValidRequest_ReturnsCreatedTournament()
    {
        // Arrange
        var expectedTournament = GetTournament();
        _mockTournamentRepository.Setup(repo => repo.CreateTournament(expectedTournament)).ReturnsAsync(expectedTournament);

        // Act
        var result = await _tournamentService.CreateTournament(expectedTournament);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task SaveTeamScore_WIthNullViewModel_ThrowsArgumentNullException()
    {
        // Act
        var serviceCall = async () => await _tournamentService.SaveTeamScore(null!);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentNullException>().WithParameterName("teamScoreViewModel");
    }

    [Fact]
    public async Task SaveTeamScore_WithValidViewModel_ReturnsScoreViewModelAndCallsRepo()
    {
        // Arrange
        var tournamentId = Guid.NewGuid().ToString();
        var tournamentScoreId = Guid.NewGuid().ToString();
        var recordedFish = new RecordedFish
        {
            Id = Guid.NewGuid().ToString(),
            ScoreId = tournamentScoreId,
            Weight = 10
        };
        var teamScoreViewModel = new TeamScoreViewModel
        {
            Id = tournamentScoreId,
            Name = "Test",
            TeamId = Guid.NewGuid().ToString(),
            TournamentId = tournamentId,
            Fish =
            [
                new()
                {
                    Id = recordedFish.Id,
                    Weight = 10,
                    ScoreId = tournamentScoreId
                }
            ]
        };

        var tournamentScore = new TournamentScore
        {
            TournamentId = tournamentId,
            Id = Guid.NewGuid().ToString(),
            TeamId = teamScoreViewModel.TeamId
        };


        _mockTournamentRepository.Setup(repo => repo.SaveTournamentScore(It.IsAny<TournamentScore>())).ReturnsAsync(tournamentScore);
        _mockTournamentRepository.Setup(repo => repo.SaveRecordedFish(It.IsAny<RecordedFish>())).ReturnsAsync(recordedFish);

        // Act
        var result = await _tournamentService.SaveTeamScore(teamScoreViewModel);

        // Assert
        result.Should().BeEquivalentTo(teamScoreViewModel);
    }

    [Fact]
    public async Task FilterTournaments_ReturnsExpectedTournamentList()
    {
        // Arrange
        var expectedTournaments = new List<Tournament> {
            GetTournament(),
            GetTournament(),
            GetTournament()
        };

        _mockTournamentRepository.Setup(repo => repo.FilterTournaments()).ReturnsAsync(expectedTournaments);

        // Act
        var result = await _tournamentService.FilterTournaments();

        // Assert
        result.Should().BeEquivalentTo(expectedTournaments);
    }

    [Fact]
    public async Task GetRegisteredTeams_ReturnsRegisteredTeamList()
    {
        // Arrange
        var tournament = GetTournament();
        var teams = GetTournamentRegistrations(tournament.Id!);

        _mockTournamentRepository.Setup(repo => repo.GetRegisteredTeams(tournament.Id!)).ReturnsAsync(teams);

        foreach (var team in teams)
        {
            _mockUserRepository.Setup(repo => repo.GetUserById(team.UserOne!)).ReturnsAsync(GetUser(team.UserOne!));
            _mockUserRepository.Setup(repo => repo.GetUserById(team.UserTwo!)).ReturnsAsync(GetUser(team.UserTwo!));
        }

        // Act
        var result = await _tournamentService.GetRegisteredTeams(tournament.Id!);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetResultPrintout_ReturnsCorrectPrintoutResult()
    {
        // Arrange
        var tournament = GetTournament();
        SetupTournament(tournament);

        // Act
        var result = await _tournamentService.GetResultPrintout(tournament.Id!);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(9);
    }

    [Fact]
    public async Task GetTournamentById_WithInvalidTournamentId_ThrowsArgumentException()
    {
        // Act
        var serviceCall = async () => await _tournamentService.GetTournamentById(string.Empty);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName("tournamentId");
    }

    [Fact]
    public async Task GetTournamentById_WithValidId_ReturnsAndCallsAllServices()
    {
        // Arrange
        var tournament = GetTournament();
        SetupTournament(tournament);

        // Act
        var result = await _tournamentService.GetTournamentById(tournament.Id!);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Id.Should().Be(tournament.Id);
    }

    [Theory]
    [MemberData(nameof(RegisterTeamTestData))]
    public async Task RegisterTeam_WIthNullRequest_ThrowsArgumentException(TournamentRegistration? tournamentRegistration, string propertyName)
    {
        // Act
        var serviceCall = async () => await _tournamentService.RegisterTeam(tournamentRegistration!);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName(propertyName);
    }

    [Fact]
    public async Task RegisterTeam_WithValidRequest_ReturnsRegisteredTeam()
    {
        // Arrange
        var team = new TournamentRegistration
        {
            Id = Guid.NewGuid().ToString(),
            Tournament = Guid.NewGuid().ToString(),
            UserOne = Guid.NewGuid().ToString(),
            UserTwo = Guid.NewGuid().ToString(),
        };

        _mockTournamentRepository.Setup(repo => repo.RegisterTeam(team)).ReturnsAsync(team);

        // Act
        var result = await _tournamentService.RegisterTeam(team);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().BeEquivalentTo(team);
    }

    [Fact]
    public async Task DeleteRegisteredTeam_WithInvalidTeamId_ThrowsArgumentException()
    {
        // Act
        var serviceCall = async () => await _tournamentService.DeleteRegisteredTeam(string.Empty);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName("teamId");
    }

    [Fact]
    public async Task DeleteRegisteredTeam_WithValidTeamId_DeletesTeamAndReturnsTrue()
    {
        // Arrange
        var teamId = Guid.NewGuid().ToString();
        _mockTournamentRepository.Setup(repo => repo.DeleteRegisteredTeam(teamId)).ReturnsAsync(true);

        // Act
        var result = await _tournamentService.DeleteRegisteredTeam(teamId);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteRecordedScore_WithInvalidScoreId_ThrowsArgumentException()
    {
        // Act
        var serviceCall = async () => await _tournamentService.DeleteRecordedScore(string.Empty);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName("scoreId");
    }

    [Fact]
    public async Task DeleteRecordedScore_WithValidScoreId_DeletesTeamAndReturnsTrue()
    {
        // Arrange
        var scoreId = Guid.NewGuid().ToString();
        _mockTournamentRepository.Setup(repo => repo.DeleteRecordedScore(scoreId)).ReturnsAsync(true);

        // Act
        var result = await _tournamentService.DeleteRecordedScore(scoreId);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteFishRecord_WithInvalidFishRecordId_ThrowsArgumentException()
    {
        // Act
        var serviceCall = async () => await _tournamentService.DeleteFishRecord(string.Empty);

        // Assert
        await serviceCall.Should().ThrowAsync<ArgumentException>().WithParameterName("recordedFishId");
    }

    [Fact]
    public async Task DeleteFishRecord_WithValidScoreId_DeletesTeamAndReturnsTrue()
    {
        // Arrange
        var fishId = Guid.NewGuid().ToString();
        _mockTournamentRepository.Setup(repo => repo.DeleteFishRecord(fishId)).ReturnsAsync(true);

        // Act
        var result = await _tournamentService.DeleteFishRecord(fishId);

        // Assert
        _mockTournamentRepository.VerifyAll();
        result.Should().BeTrue();
    }

    public static readonly TheoryData<TournamentRegistration, string> RegisterTeamTestData = new()
    {
        { null!, "tournamentRegistration" },
        {
            new TournamentRegistration
            {
                UserOne = Guid.NewGuid().ToString(),
                UserTwo = Guid.NewGuid().ToString(),
            },
            nameof(TournamentRegistration.Tournament)
        },
        {
            new TournamentRegistration
            {
                Tournament = Guid.NewGuid().ToString(),
                UserTwo = Guid.NewGuid().ToString(),
            },
            nameof(TournamentRegistration.UserOne)
        },
        {
            new TournamentRegistration
            {
                Tournament = Guid.NewGuid().ToString(),
                UserOne = Guid.NewGuid().ToString(),
            },
            nameof(TournamentRegistration.UserTwo)
        },
    };

    public static readonly TheoryData<Tournament, string> CreateTournamentTestData = new()
    {
        { null!, "tournament" },
        {
            new Tournament
            {
                Id = Guid.NewGuid().ToString(),
                Lake = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
            },
            nameof(Tournament.Date)
        },
        {
            new Tournament
            {
                Id = Guid.NewGuid().ToString(),
                Lake = Guid.NewGuid().ToString(),
                Date = DateTime.Now
            },
            nameof(Tournament.Name)
        },
        {
            new Tournament
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Date = DateTime.Now
            },
            nameof(Tournament.Lake)
        }
    };

    private void SetupTournament(Tournament tournament)
    {
        _mockTournamentRepository.Setup(repo => repo.GetTournamentById(tournament.Id!)).ReturnsAsync(tournament);

        var teams = GetTournamentRegistrations(tournament.Id!);
        _mockTournamentRepository.Setup(repo => repo.GetRegisteredTeams(tournament.Id!)).ReturnsAsync(teams);

        foreach (var team in teams)
        {
            _mockUserRepository.Setup(repo => repo.GetUserById(team.UserOne!)).ReturnsAsync(GetUser(team.UserOne!));
            _mockUserRepository.Setup(repo => repo.GetUserById(team.UserTwo!)).ReturnsAsync(GetUser(team.UserTwo!));
        }

        var scores = teams.SelectMany(team => GetTournamentScores(tournament.Id!, team.Id!)).ToList();

        foreach (var scoreGroup in scores.GroupBy(score => score.TournamentId).ToList())
        {
            _mockTournamentRepository.Setup(repo => repo.GetResults(scoreGroup.Key!)).ReturnsAsync([.. scoreGroup]);
        }

        var fish = scores.SelectMany(score => GetRecordedFish(score.Id!)).ToList();

        foreach (var fishGroup in fish.GroupBy(f => f.ScoreId!).ToList())
        {
            _mockTournamentRepository.Setup(repo => repo.GetRecordedFish(fishGroup.Key!)).ReturnsAsync([.. fishGroup]);
        }
    }

    private static IEnumerable<RecordedFish> GetRecordedFish(string scoreId)
    {
        return
        [
            new RecordedFish
            {
                Id = Guid.NewGuid().ToString(),
                ScoreId = scoreId,
                Weight = 10
            },
            new RecordedFish
            {
                Id = Guid.NewGuid().ToString(),
                ScoreId = scoreId,
                Weight = 10
            },
            new RecordedFish
            {
                Id = Guid.NewGuid().ToString(),
                ScoreId = scoreId,
                Weight = 10
            },
        ];
    }

    private static IEnumerable<TournamentScore> GetTournamentScores(string tournamentId, string teamId)
    {
        return
        [
            new TournamentScore
            {
                Id = Guid.NewGuid().ToString(),
                TournamentId = tournamentId,
                TeamId = teamId
            },
            new TournamentScore
            {
                Id = Guid.NewGuid().ToString(),
                TournamentId = tournamentId,
                TeamId = teamId
            },
            new TournamentScore
            {
                Id = Guid.NewGuid().ToString(),
                TournamentId = tournamentId,
                TeamId = teamId
            },
        ];
    }

    private static User GetUser(string userId)
    {
        return new User
        {
            Id = userId,
            Birthday = DateTime.Now,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Grade = 10
        };
    }

    private static IEnumerable<TournamentRegistration> GetTournamentRegistrations(string tournamentId)
    {
        return
        [
            new TournamentRegistration
            {
                Id = Guid.NewGuid().ToString(),
                Tournament = tournamentId,
                UserOne = Guid.NewGuid().ToString(),
                UserTwo = Guid.NewGuid().ToString(),
            },
            new TournamentRegistration
            {
                Id = Guid.NewGuid().ToString(),
                Tournament = tournamentId,
                UserOne = Guid.NewGuid().ToString(),
                UserTwo = Guid.NewGuid().ToString(),
            },
            new TournamentRegistration
            {
                Id = Guid.NewGuid().ToString(),
                Tournament = tournamentId,
                UserOne = Guid.NewGuid().ToString(),
                UserTwo = Guid.NewGuid().ToString(),
            },
        ];
    }

    private static Tournament GetTournament()
    {
        return new Tournament
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
            Lake = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
        };
    }
}
