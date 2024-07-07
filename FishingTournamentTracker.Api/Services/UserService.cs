using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;

namespace FishingTournamentTracker.Api.Services;

public class UserService(IUserRepository userRepository, ITournamentService tournamentService, IFileParser fileParser) : IUserService
{
    /// <summary>
    /// Loaded way to delete a user, but i'm bound by the design of my base repo
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> Delete(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var tournaments = await tournamentService.FilterTournaments();
        var fullTournamentTasks = tournaments.Select(tournament => tournamentService.GetTournamentById(tournament.Id!)).ToList();

        await Task.WhenAll(fullTournamentTasks);

        var hydratedTournaments = fullTournamentTasks.Select(task => task.Result).ToList();
        var tournamentsByUser = hydratedTournaments
            .Where(tournament => tournament.RegisteredTeams!.Any(team => team.UserOne!.Id == userId || team.UserTwo!.Id == userId))
            .ToList();

        var teams = tournamentsByUser.SelectMany(tournament => tournament.RegisteredTeams!
            .Where(team => team.UserOne!.Id == userId || team.UserTwo!.Id == userId))
            .ToList();

        var scores = tournamentsByUser.SelectMany(tournament => tournament.Results!
            .Where(result => teams.Any(team => team.Id == result.TeamId)))
            .ToList();

        var fish = scores.SelectMany(score => score.Fish.Select(fish => fish.Id))
            .ToList();

        await Task.WhenAll(teams.Select(team => tournamentService.DeleteRegisteredTeam(team.Id!)).ToList());
        await Task.WhenAll(scores.Select(score => tournamentService.DeleteRecordedScore(score.Id!)).ToList());
        await Task.WhenAll(fish.Select(fish => tournamentService.DeleteFishRecord(fish!)).ToList());

        return await userRepository.Delete(userId);
    }

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
