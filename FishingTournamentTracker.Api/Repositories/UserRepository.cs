using Dapper;
using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Utility;
using Microsoft.Extensions.Options;

namespace FishingTournamentTracker.Api.Repositories;

public class UserRepository(IOptions<DatabaseConfiguration> databaseConfiguration) : BaseRepository(databaseConfiguration), IUserRepository
{
    public async Task<User> AddUser(User user)
    {
        return await Insert(user);
    }

    public async Task<int> CountUsers(UserFilter userFilter)
    {
        return await Count<User>(BuildDynamicParameters(userFilter));
    }

    public async Task<IEnumerable<User>> FilterUsers(UserFilter userFilter)
    {
        return await Search<User>(BuildDynamicParameters(userFilter));
    }

    public async Task<User> GetUserById(string userId)
    {
        return await FindById<User>(userId);
    }

    public async Task<User> UpdateUser(User user)
    {
        return await Update(user);
    }

    private DynamicParameters BuildDynamicParameters(UserFilter userFilter)
    {
        var dynamicParameters = new DynamicParameters();

        if (userFilter != null)
        {
            if (!string.IsNullOrWhiteSpace(userFilter.Name))
            {
                dynamicParameters.Add("FirstName", userFilter.Name);
            }

            if (userFilter.Grade is not null)
            {
                dynamicParameters.Add(nameof(userFilter.Grade), userFilter.Grade);
            }

            if (userFilter.Page is not null)
            {
                CurrentPage = userFilter.Page;
            }

            if (userFilter.PageSize is not null)
            {
                PageSize = userFilter.PageSize;
            }
        }

        return dynamicParameters;
    }
}
