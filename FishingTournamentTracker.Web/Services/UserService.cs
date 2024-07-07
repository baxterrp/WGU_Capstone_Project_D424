using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Library.Utility;
using FishingTournamentTracker.Web.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace FishingTournamentTracker.Web.Services;

public class UserService(HttpClient httpClient, ITokenService tokenService, IMemoryCache memoryCache) 
    : BaseHttpClientService(httpClient, tokenService, memoryCache), IUserService
{
    private const string _userApiRoute = "api/user";

    public async Task<User?> GetUserById(string userId)
    {
        return await TrySendHttpRequest<User>(new HttpRequestMessage(HttpMethod.Get, $"{_userApiRoute}/{userId}"));
    }

    public async Task<User?> AddUser(User user)
    {
        return await TrySendHttpRequest<User>(new HttpRequestMessage(HttpMethod.Post, _userApiRoute)
        {
            Content = user.ToHttpContent()
        });
    }

    public async Task<User?> EditUser(User user)
    {
        return await TrySendHttpRequest<User>(new HttpRequestMessage(HttpMethod.Put, _userApiRoute)
        {
            Content= user.ToHttpContent()
        });
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await TrySendHttpRequest<IEnumerable<User>>(new HttpRequestMessage(HttpMethod.Get, _userApiRoute)) ?? [];
    }

    public async Task<PaginatedResult<User>?> FilterUsers(string? name, string? grade, int? page, int? size)
    {
        var queryParameters = new List<(string ParameterName, object? ParameterValue)>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            queryParameters.Add((nameof(name), name));
        }

        if (!string.IsNullOrWhiteSpace(grade))
        {
            queryParameters.Add((nameof(grade), grade));
        }

        if (page is not null)
        {
            queryParameters.Add((nameof(page), page));
        }

        if (size is not null)
        {
            queryParameters.Add((nameof(size), size));
        }

        var apiUrl = $"{_userApiRoute}/filter";

        if (queryParameters.Count != 0)
        {
            apiUrl += "?" + string.Join("&", queryParameters.Select(parameter => $"{parameter.ParameterName}={parameter.ParameterValue}"));
        }

        return await TrySendHttpRequest<PaginatedResult<User>>(new HttpRequestMessage(HttpMethod.Get, apiUrl));
    }

    public async Task<IEnumerable<User>?> AutomatedUserUpload(string upload)
    {
        var fileUpload = new FileUpload
        {
            Contents = upload
        };

        return await TrySendHttpRequest<IEnumerable<User>>(new HttpRequestMessage(HttpMethod.Post, $"{_userApiRoute}/automated")
        {
            Content = fileUpload.ToHttpContent(),
        });
    }
}
