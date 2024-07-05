using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;

namespace FishingTournamentTracker.Web.Services;

public interface ITournamentService
{
    Task<IEnumerable<Tournament>> SearchTournaments();
    Task<TournamentViewModel?> GetTournamentById(string tournamentId);
    Task<Tournament?> CreateTournament(Tournament tournament);
    Task<TournamentRegistration?> RegisterTeam(TournamentRegistration teamRegistration);
    Task<TeamScoreViewModel?> SaveTeamScore(TeamScoreViewModel teamScore);
}
