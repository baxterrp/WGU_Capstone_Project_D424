using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Web.Services;

public interface IUserService
{
    Task<User?> AddUser(User user);
    Task<User?> EditUser(User user);
    Task<IEnumerable<User>> GetUsers(string? name, string? grade);
    Task<User?> GetUserById(string userId);
    Task<IEnumerable<User>?> AutomatedUserUpload(string upload);
}
