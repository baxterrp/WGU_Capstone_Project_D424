namespace FishingTournamentTracker.Web.Services;

public class TokenService(HttpClient httpClient) : ITokenService
{
    private const string _tokenApiUrl = "api/token";

    public async Task<string> GetApiToken()
    {
        var httpResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, _tokenApiUrl));

        if (httpResponse.IsSuccessStatusCode)
        {
            return await httpResponse.Content.ReadAsStringAsync();
        }

        return string.Empty;
    }
}
