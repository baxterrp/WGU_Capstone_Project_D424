using FishingTournamentTracker.Library.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FishingTournamentTracker.Web.Pages;

public partial class Tournament
{
    [Parameter]
    public string? TournamentId {  get; set; }

    public TournamentViewModel? SelectedTournament { get; set; }

    public bool RecordScoreIsDisabled
    {
        get
        {
            return !SelectedTournament?.RegisteredTeams?.Any() ?? true;
        }
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SelectedTournament = await tournamentService.GetTournamentById(TournamentId!);
    }

    private void GoToRecordScorePage()
    {
        navigationManager.NavigateTo($"/tournament/{TournamentId}/record");
    }

    private void RegisterTeam(MouseEventArgs e)
    {
        navigationManager.NavigateTo($"/tournament/{TournamentId}/registration");
    }
}