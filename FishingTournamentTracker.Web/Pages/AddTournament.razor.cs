namespace FishingTournamentTracker.Web.Pages;

public partial class AddTournament
{
    public Library.Models.DataModels.Tournament? SelectedTournament { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SelectedTournament ??= new();
    }

    private async Task OnSubmit()
    {
        await tournamentService.CreateTournament(SelectedTournament!);

        SelectedTournament = new();
    }
}
