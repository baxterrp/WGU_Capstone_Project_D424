using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Web.Constants;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FishingTournamentTracker.Web.Services;

public abstract class BaseHttpClientService(HttpClient httpClient, ITokenService tokenService, IMemoryCache memoryCache)
{
    public Admin? Admin { private get;  set; }

    protected async Task<TEntity?> TrySendHttpRequest<TEntity>(HttpRequestMessage httpRequestMessage)
    {
        try
        {
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiToken());
            var httpResponse = await httpClient.SendAsync(httpRequestMessage);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TEntity>(stringResponse);
        }
        catch
        {
            return default!;
        }
    }

    protected async Task<byte[]> TrySendFileDownload(HttpRequestMessage httpRequestMessage)
    {
        try
        {
            var httpResponse = await httpClient.SendAsync(httpRequestMessage);
            return await httpResponse.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return default!;
        }
    }

    private async Task<string> GetApiToken()
    {
        if (memoryCache.TryGetValue(HttpValues.ApiToken, out string? parsedToken))
        {
            return parsedToken!;
        }

        var token = await tokenService.GetApiToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            memoryCache.Set(HttpValues.ApiToken, token, new DateTimeOffset(DateTime.Now.AddHours(1)));
        }

        return token;
    }
}
