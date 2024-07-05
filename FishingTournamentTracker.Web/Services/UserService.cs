using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Web.Extensions;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Web;

namespace FishingTournamentTracker.Web.Services;

public class UserService(HttpClient httpClient) : BaseHttpClientService(httpClient), IUserService
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

    public async Task<IEnumerable<User>> GetUsers(string? name, string? grade)
    {
        var queryParameters = new List<string?> { name, grade }
            .Where(parameter => !string.IsNullOrWhiteSpace(parameter));
        var apiUrl = _userApiRoute;

        if (queryParameters.Any())
        {
            apiUrl += "?" + string.Join("&", queryParameters.Select(parameter => $"{nameof(parameter)}={parameter}"));
        }

        return await TrySendHttpRequest<IEnumerable<User>>(new HttpRequestMessage(HttpMethod.Get, apiUrl)) ?? [];
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
