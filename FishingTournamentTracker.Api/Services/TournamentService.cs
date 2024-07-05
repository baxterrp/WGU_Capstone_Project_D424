using FishingTournamentTracker.Api.Extensions;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace FishingTournamentTracker.Api.Services;
public class TournamentService(ITournamentRepository tournamentRepository, IUserService userService) : ITournamentService
{
    public async Task<Tournament> CreateTournament(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament, nameof(tournament));
        ArgumentNullException.ThrowIfNull(tournament.Date, nameof(tournament.Date));
        ArgumentException.ThrowIfNullOrWhiteSpace(tournament.Name, nameof(tournament.Name));
        ArgumentException.ThrowIfNullOrWhiteSpace(tournament.Lake, nameof(tournament.Lake));

        return await tournamentRepository.CreateTournament(tournament);
    }

    public async Task<TeamScoreViewModel> SaveTeamScore(TeamScoreViewModel teamScoreViewModel)
    {
        ArgumentNullException.ThrowIfNull(teamScoreViewModel);

        var tournamentScore = await tournamentRepository.SaveTournamentScore(teamScoreViewModel.ToTeamScore());

        foreach (var fish in teamScoreViewModel.Fish)
        {
            var savedFishRecord = await tournamentRepository.SaveRecordedFish(fish.ToRecordedFish(tournamentScore));
            fish.Id = savedFishRecord.Id;
        }

        teamScoreViewModel.Id = tournamentScore.Id;

        return teamScoreViewModel;
    }

    public async Task<IEnumerable<Tournament>> FilterTournaments()
    {
        return await tournamentRepository.FilterTournaments();
    }

    public async Task<IEnumerable<TeamViewModel>> GetRegisteredTeams(string tournamentId)
    {
        var registrations = await tournamentRepository.GetRegisteredTeams(tournamentId);
        var registeredTeams = new List<TeamViewModel>();

        foreach (var registration in registrations)
        {
            registeredTeams.Add(new TeamViewModel
            {
                Id = registration.Id,
                UserOne = await userService.GetUserById(registration.UserOne!),
                UserTwo = await userService.GetUserById(registration.UserTwo!),
            });
        }

        return registeredTeams;
    }

    public async Task<IEnumerable<TeamScoreViewModel>> GetResults(string tournamentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tournamentId);

        var scores = await tournamentRepository.GetResults(tournamentId);
        var teams = await tournamentRepository.GetRegisteredTeams(tournamentId);

        var results = new List<TeamScoreViewModel>();

        foreach(var score in scores)
        {
            var team = teams.FirstOrDefault(t => t.Id == score.TeamId);
            var userOne = await userService.GetUserById(team!.UserOne!);
            var userTwo = await userService.GetUserById(team.UserTwo!);

            if (team is null) { continue; }

            results.Add(new TeamScoreViewModel
            {
                Id = score.Id,
                Name = (userOne, userTwo).GetTeamName(),
                TeamId = team.Id,
                TournamentId = tournamentId,
                Fish = (await tournamentRepository.GetRecordedFish(score.Id!))?.Select(fish => new RecordedFishViewModel
                {
                    Id = fish.Id,
                    ScoreId = score.Id,
                    Weight = fish.Weight
                }).ToList() ?? []
            });
        }

        return results;
    }

    public async Task<TournamentViewModel> GetTournamentById(string tournamentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tournamentId, nameof(tournamentId));
        var tournament = await tournamentRepository.GetTournamentById(tournamentId);
        var registeredTeams = await GetRegisteredTeams(tournamentId);
        var results = await GetResults(tournamentId);

        return tournament.ToTournamentViewModel([.. registeredTeams], [.. results]);
    }

    public async Task<TournamentRegistration> RegisterTeam(TournamentRegistration tournamentRegistration)
    {
        ArgumentNullException.ThrowIfNull(tournamentRegistration);
        ArgumentException.ThrowIfNullOrWhiteSpace(tournamentRegistration.Tournament);
        ArgumentException.ThrowIfNullOrWhiteSpace(tournamentRegistration.UserOne);
        ArgumentException.ThrowIfNullOrWhiteSpace(tournamentRegistration.UserTwo);

        return await tournamentRepository.RegisterTeam(tournamentRegistration);
    }
}
