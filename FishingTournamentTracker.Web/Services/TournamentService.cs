﻿using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using FishingTournamentTracker.Web.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace FishingTournamentTracker.Web.Services;

public class TournamentService(HttpClient httpClient, ITokenService tokenService, IMemoryCache memoryCache) 
    : BaseHttpClientService(httpClient, tokenService, memoryCache), ITournamentService
{
    private const string _tournamentApiRoute = "api/tournament";

    public async Task<Tournament?> CreateTournament(Tournament tournament)
    {
        return await TrySendHttpRequest<Tournament>(new HttpRequestMessage(HttpMethod.Post, _tournamentApiRoute)
        {
            Content = tournament.ToHttpContent()
        });
    }

    public async Task<byte[]?> DownloadResultsSpreadsheet(string tournamentId)
    {
        return await TrySendFileDownload(new HttpRequestMessage(HttpMethod.Get, $"{_tournamentApiRoute}/download/{tournamentId}"));
    }

    public async Task<TournamentViewModel?> GetTournamentById(string tournamentId)
    {
        return await TrySendHttpRequest<TournamentViewModel>(new HttpRequestMessage(HttpMethod.Get, $"{_tournamentApiRoute}/{tournamentId}"));
    }

    public async Task<IEnumerable<TournamentResultPrintout>?> GetTournamentResultPrintout(string tournamentId)
    {
        return await TrySendHttpRequest<IEnumerable<TournamentResultPrintout>>(new HttpRequestMessage(HttpMethod.Get, $"{_tournamentApiRoute}/result/{tournamentId}"));
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
