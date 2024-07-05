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

    public async Task<IEnumerable<User>> FilterUsers(UserFilter userFilter)
    {
        var dynamicParameters = new DynamicParameters();

        if (userFilter != null)
        {
            if (!string.IsNullOrWhiteSpace(userFilter.Name))
            {
                dynamicParameters.Add(nameof(UserFilter.Name), userFilter.Name);
            }

            if (userFilter.Grade is not null)
            {
                dynamicParameters.Add(nameof(userFilter.Grade), userFilter.Grade);
            }
        }

        return await Search<User>(dynamicParameters);
    }

    public async Task<User> GetUserById(string userId)
    {
        return await FindById<User>(userId);
    }

    public async Task<User> UpdateUser(User user)
    {
        return await Update(user);
    }
}
