namespace FishingTournamentTracker.Web.Pages;

public partial class Tournaments
{
    public List<Library.Models.DataModels.Tournament>? TournamentList { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

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
