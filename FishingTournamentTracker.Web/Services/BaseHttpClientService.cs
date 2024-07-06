using System.Text.Json;

namespace FishingTournamentTracker.Web.Services;

public abstract class BaseHttpClientService(HttpClient httpClient)
{
    protected async Task<TEntity?> TrySendHttpRequest<TEntity>(HttpRequestMessage httpRequestMessage)
    {
        try
        {
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
}
