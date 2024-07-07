using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class Tournaments
{
    [CascadingParameter]
    public Library.Models.DataModels.Admin? LoggedInAdministrator { get; set; }

    public List<Library.Models.DataModels.Tournament>? TournamentList { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (string.IsNullOrWhiteSpace(LoggedInAdministrator?.Id))
        {
            navigationManager.NavigateTo("/");
        }

        try
        {
            var tournaments = await tournamentService.SearchTournaments();
            TournamentList = tournaments?.ToList() ?? [];
        }
        catch
        {
            TournamentList = [];
        }
    }

    private void GoToTournamentPage(string tournamentId)
    {
        navigationManager.NavigateTo($"/tournament/{tournamentId}");
    }
}
