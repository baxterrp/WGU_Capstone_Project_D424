using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;

namespace FishingTournamentTracker.Api.Extensions;

public static class MapperExtensions
{
    public static IEnumerable<TournamentResultPrintout> ToTournamentResultPrintout(this TournamentViewModel tournament)
    {
        var place = 1;
        var sortedResults = tournament.Results!.OrderByDescending(result => result.Fish.Sum(f => f.Weight)).ToList();
        var printouts = new List<TournamentResultPrintout>();

        foreach (var result in sortedResults)
        {
            var hasFish = result.Fish?.Any() ?? false;
            var fishCount = hasFish ? $"{result.Fish!.Count(fish => fish.Weighed)}/{result.Fish!.Count}" : string.Empty;
            var biggestFish = hasFish ? result.Fish!.Max(fish => fish.Weight) : 0;
            var totalWeight = hasFish ? result.Fish!.Sum(fish => fish.Weight) : 0;
            var points = sortedResults.Count - (place - 1);

            printouts.Add(new TournamentResultPrintout
            {
                Place = place,
                Team = result.Name,
                Fish = fishCount,
                BiggestFish = biggestFish,
                TotalWeight = totalWeight,
                Points = points,
            });

            place++;
        }

        return printouts;
    }
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
