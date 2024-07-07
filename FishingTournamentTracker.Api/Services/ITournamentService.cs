using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;

namespace FishingTournamentTracker.Api.Services;

public interface ITournamentService
{
    Task<Tournament> CreateTournament(Tournament tournament);
    Task<IEnumerable<Tournament>> FilterTournaments();
    Task<TournamentViewModel> GetTournamentById(string tournamentId);
    Task<TournamentRegistration> RegisterTeam(TournamentRegistration tournamentRegistration);
    Task<IEnumerable<TeamViewModel>> GetRegisteredTeams(string tournamentId);
    Task<TeamScoreViewModel> SaveTeamScore(TeamScoreViewModel teamScoreViewModel);
    Task<IEnumerable<TeamScoreViewModel>> GetResults(string tournamentId);
    Task<IEnumerable<TournamentResultPrintout>> GetResultPrintout(string tournamentId);
    Task<byte[]> DownloadResultExcel(string tournamentId);
    Task<bool> DeleteTournament(string tournamentId);
    Task<bool> DeleteRegisteredTeam(string teamId);
    Task<bool> DeleteRecordedScore(string scoreId);
    Task<bool> DeleteFishRecord(string recordedFishId);
}
