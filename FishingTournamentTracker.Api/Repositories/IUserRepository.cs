using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Utility;

namespace FishingTournamentTracker.Api.Repositories;

public interface IUserRepository
{
    Task<User> AddUser(User user);
    Task<IEnumerable<User>> FilterUsers(UserFilter userFilter);
    Task<User> UpdateUser(User user);
    Task<User> GetUserById(string userId);
    Task<int> CountUsers(UserFilter userFilter);
}
