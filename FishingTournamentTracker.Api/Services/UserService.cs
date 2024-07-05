using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;

namespace FishingTournamentTracker.Api.Services;

public class UserService(IUserRepository userRepository, IFileParser fileParser) : IUserService
{
    public async Task<IEnumerable<User>> BulkUserUpload(FileUpload upload)
    {
        ArgumentNullException.ThrowIfNull(upload);
        ArgumentException.ThrowIfNullOrWhiteSpace(upload.Contents);

        var users = fileParser.ParseExcel<User>(Convert.FromBase64String(upload.Contents));
        return await Task.WhenAll(users.Select(user => userRepository.AddUser(user)).ToList());
    }

    public async Task<User> AddUser(User user)
    {
        ValidateUser(user);

        return await userRepository.AddUser(user);
    }

    public async  Task<User> EditUser(User user)
    {
        ValidateUser(user);

        return await userRepository.UpdateUser(user);
    }

    public async Task<IEnumerable<User>> GetAllUsers(UserFilter userFilter)
    {
        return await userRepository.FilterUsers(userFilter);
    }

    public async Task<PaginatedResult<User>> FilterUsers(UserFilter userFilter)
    {
        var users = await userRepository.FilterUsers(userFilter);
        var count = await userRepository.CountUsers(userFilter);

        return new PaginatedResult<User>
        {
            PageNumber = userFilter.Page,
            PageSize = userFilter.PageSize,
            Data = users,
            TotalPages = (int)Math.Ceiling((decimal)count / userFilter.PageSize!.Value)
        };
    }

    public async Task<User> GetUserById(string userId)
    {
        return await userRepository.GetUserById(userId);
    }

    private static void ValidateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentException.ThrowIfNullOrWhiteSpace(user.FirstName, nameof(user.FirstName));
    }

}
