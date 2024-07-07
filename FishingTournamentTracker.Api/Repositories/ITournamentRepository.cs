using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Api.Repositories;

public interface ITournamentRepository
{
    Task<Tournament> CreateTournament(Tournament tournament);
    Task<IEnumerable<Tournament>> FilterTournaments();
    Task<Tournament> GetTournamentById(string tournamentId);
    Task<TournamentRegistration> RegisterTeam(TournamentRegistration teamRegistration);
    Task<IEnumerable<TournamentRegistration>> GetRegisteredTeams(string tournamentId);
    Task<TournamentScore> SaveTournamentScore(TournamentScore tournamentScore);
    Task<RecordedFish> SaveRecordedFish(RecordedFish recordedFish);
    Task<IEnumerable<TournamentScore>> GetResults(string tournamentId);
    Task<IEnumerable<RecordedFish>> GetRecordedFish(string scoreId);
    Task<bool> DeleteTournament(string tournamentId);
    Task<bool> DeleteRegisteredTeam(string teamId);
    Task<bool> DeleteRecordedScore(string scoreId);
    Task<bool> DeleteFishRecord(string recordedFishId);
}
