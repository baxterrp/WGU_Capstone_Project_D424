namespace FishingTournamentTracker.Web.Services;

public interface ITokenService
{
    Task<string> GetApiToken();
}
