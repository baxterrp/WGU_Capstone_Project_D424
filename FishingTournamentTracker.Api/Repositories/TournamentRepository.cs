using Dapper;
using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.Extensions.Options;

namespace FishingTournamentTracker.Api.Repositories;

public class TournamentRepository(IOptions<DatabaseConfiguration> databaseConfiguration) : BaseRepository(databaseConfiguration), ITournamentRepository
{
    public async Task<Tournament> CreateTournament(Tournament tournament)
    {
        return await Insert(tournament);
    }

    public async Task<IEnumerable<Tournament>> FilterTournaments()
    {
        return await Search<Tournament>(new DynamicParameters());
    }

    public async Task<IEnumerable<RecordedFish>> GetRecordedFish(string scoreId)
    {
        var parameters = new DynamicParameters();
        parameters.Add(nameof(scoreId), scoreId);

        return await Search<RecordedFish>(parameters);
    }

    public async Task<IEnumerable<TournamentScore>> GetResults(string tournamentId)
    {
        var parameters = new DynamicParameters();
        parameters.Add(nameof(tournamentId), tournamentId);

        return await Search<TournamentScore>(parameters);
    }

    public async Task<IEnumerable<TournamentRegistration>> GetRegisteredTeams(string tournamentId)
    {
        var parameters = new DynamicParameters();
        parameters.Add(nameof(Tournament), tournamentId);

        return await Search<TournamentRegistration>(parameters);
    }

    public async Task<Tournament> GetTournamentById(string tournamentId)
    {
        return await FindById<Tournament>(tournamentId);
    }

    public async Task<TournamentRegistration> RegisterTeam(TournamentRegistration teamRegistration)
    {
        return await Insert(teamRegistration);
    }

    public async Task<RecordedFish> SaveRecordedFish(RecordedFish recordedFish)
    {
        return await Insert(recordedFish);
    }

    public async Task<TournamentScore> SaveTournamentScore(TournamentScore tournamentScore)
    {
        return await Insert(tournamentScore);
    }

    public async Task<bool> DeleteTournament(string tournamentId)
    {
        try
        {
            await Delete<Tournament>(tournamentId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteRegisteredTeam(string teamId)
    {
        try
        {
            await Delete<TournamentRegistration>(teamId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteRecordedScore(string scoreId)
    {
        try
        {
            await Delete<TournamentScore>(scoreId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteFishRecord(string recordedFishId)
    {
        try
        {
            await Delete<RecordedFish>(recordedFishId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
