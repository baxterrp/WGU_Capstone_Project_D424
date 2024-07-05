using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace FishingTournamentTracker.Web.Extensions;

public static class HttpRequestExtensions
{
    public static HttpContent? ToHttpContent<TEntity>(this TEntity entity)
    {
        return new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}
