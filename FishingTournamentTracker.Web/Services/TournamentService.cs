using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Web.Extensions;

namespace FishingTournamentTracker.Web.Services;

public class TournamentService(HttpClient httpClient) : BaseHttpClientService(httpClient), ITournamentService
{
    private const string _tournamentApiRoute = "api/tournament";

    public async Task<Tournament?> CreateTournament(Tournament tournament)
    {
        return await TrySendHttpRequest<Tournament>(new HttpRequestMessage(HttpMethod.Post, _tournamentApiRoute)
        {
            Content = tournament.ToHttpContent()
        });
    }

    public async Task<TournamentViewModel?> GetTournamentById(string tournamentId)
    {
        return await TrySendHttpRequest<TournamentViewModel>(new HttpRequestMessage(HttpMethod.Get, $"{_tournamentApiRoute}/{tournamentId}"));
    }

    public async Task<TournamentRegistration?> RegisterTeam(TournamentRegistration teamRegistration)
    {
        return await TrySendHttpRequest<TournamentRegistration>(new HttpRequestMessage(HttpMethod.Post, $"{_tournamentApiRoute}/register")
        {
            Content = teamRegistration.ToHttpContent()
        });
    }

    public async Task<TeamScoreViewModel?> SaveTeamScore(TeamScoreViewModel teamScore)
    {
        return await TrySendHttpRequest<TeamScoreViewModel>(new HttpRequestMessage(HttpMethod.Post, $"{_tournamentApiRoute}/result")
        {
            Content = teamScore.ToHttpContent()
        });
    }

    public async Task<IEnumerable<Tournament>> SearchTournaments()
    {
        return await TrySendHttpRequest<IEnumerable<Tournament>>(new HttpRequestMessage(HttpMethod.Get, _tournamentApiRoute)) ?? [];
    }
}
