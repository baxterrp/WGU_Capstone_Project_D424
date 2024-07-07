using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Web.Extensions;
using System.Text.Json;

namespace FishingTournamentTracker.Web.Services;

public class AdminService(HttpClient httpClient) : IAdminService
{
    private const string _adminApiUrl = "api/admin";

    public async Task<Admin?> Login(Admin admin)
    {
        return await SendAdminRequest(new HttpRequestMessage(HttpMethod.Post, $"{_adminApiUrl}/login")
        {
            Content = admin.ToHttpContent()
        });
    }

    public async Task<Admin?> Register(Admin admin)
    {
        return await SendAdminRequest(new HttpRequestMessage(HttpMethod.Post, $"{_adminApiUrl}/register")
        {
            Content = admin.ToHttpContent()
        });
    }

    private async Task<Admin?> SendAdminRequest(HttpRequestMessage requestMessage)
    {
        var response = await httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<Admin>(await response.Content.ReadAsStringAsync());
        }

        return null;
    }
}
