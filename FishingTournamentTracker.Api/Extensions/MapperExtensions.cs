using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;

namespace FishingTournamentTracker.Api.Extensions;

public static class MapperExtensions
{
    public static TournamentViewModel ToTournamentViewModel(this Tournament tournament, List<TeamViewModel> registeredTeams, List<TeamScoreViewModel> teamScores)
    {
        return new TournamentViewModel
        {
            Id = tournament.Id,
            Date = tournament.Date,
            Lake = tournament.Lake,
            Name = tournament.Name,
            RegisteredTeams = registeredTeams,
            Results = teamScores
        };
    }

    public static TournamentScore ToTeamScore(this TeamScoreViewModel tournamentScore)
    {
        return new TournamentScore
        {
            TeamId = tournamentScore.TeamId,
            TournamentId = tournamentScore.TournamentId
        };
    }

    public static RecordedFish ToRecordedFish(this RecordedFishViewModel fishViewModel, TournamentScore tournamentScore)
    {
        return new RecordedFish
        {
            ScoreId = tournamentScore.Id,
            Weight = fishViewModel.Weight,
        };
    }

    public static string GetTeamName(this (User UserOne, User UserTwo) team)
    {
        return string.Join("-", $"{team.UserOne!.FirstName} {team.UserOne.LastName}", $"{team.UserTwo!.FirstName} {team.UserTwo.LastName}");
    }
}
