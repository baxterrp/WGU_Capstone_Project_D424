using FishingTournamentTracker.Library.Exeptions;
using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Web.Constants;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FishingTournamentTracker.Web.Services;

public abstract class BaseHttpClientService(HttpClient httpClient, ITokenService tokenService, IMemoryCache memoryCache)
{
    public Admin? Admin { private get; set; }

    private const int OneSecond = 1000;
    private const int ThreeSeconds = 3000;
    private const int FiveSeconds = 5000;
    private const string BearerHeader = "Bearer";

    protected async Task<TEntity?> TrySendHttpRequest<TEntity>(HttpRequestMessage httpRequestMessage)
    {
        try
        {
            var httpResponse = await SendRequest(httpRequestMessage);
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
            var httpResponse = await SendRequest(httpRequestMessage);
            return await httpResponse.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return default!;
        }
    }

    protected async Task<HttpResponseMessage> SendRequest(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(BearerHeader, await GetApiToken());
        
        for (var i = 0; i < 3; i++)
        {
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                switch (i)
                {
                    case 0:
                        await Task.Delay(OneSecond);
                        memoryCache.Remove(HttpValues.ApiToken);
                        continue;
                    case 1:
                        await Task.Delay(ThreeSeconds);
                        memoryCache.Remove(HttpValues.ApiToken);
                        continue;
                    case 2:
                        await Task.Delay(FiveSeconds);
                        memoryCache.Remove(HttpValues.ApiToken);
                        continue;
                }
            }

            return response;
        }

        throw new UnauthorizedApiAccessException();
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
            memoryCache.Set(HttpValues.ApiToken, token, new DateTimeOffset(DateTime.Now.AddMinutes(1)));
        }

        return token;
    }
}
