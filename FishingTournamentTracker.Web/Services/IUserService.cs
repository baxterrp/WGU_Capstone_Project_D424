using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Utility;

namespace FishingTournamentTracker.Web.Services;

public interface IUserService
{
    Task<User?> AddUser(User user);
    Task<User?> EditUser(User user);
    Task<PaginatedResult<User>?> FilterUsers(string? name, string? grade, int? page, int? size);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetUserById(string userId);
    Task<IEnumerable<User>?> AutomatedUserUpload(string upload);
}
