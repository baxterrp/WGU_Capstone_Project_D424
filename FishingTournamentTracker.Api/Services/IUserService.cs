using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;

namespace FishingTournamentTracker.Api.Services;

public interface IUserService
{
    Task<User> AddUser(User user);
    Task<User> EditUser(User user);
    Task<IEnumerable<User>> GetAllUsers(UserFilter userFilter);
    Task<PaginatedResult<User>> FilterUsers(UserFilter userFilter);
    Task<User> GetUserById(string userId);
    Task<IEnumerable<User>> BulkUserUpload(FileUpload upload);
}
