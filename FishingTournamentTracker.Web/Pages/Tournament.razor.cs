using FishingTournamentTracker.Library.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace FishingTournamentTracker.Web.Pages;

public partial class Tournament
{
    [Parameter]
    public string? TournamentId {  get; set; }

    public TournamentViewModel? SelectedTournament { get; set; }

    public List<TournamentResultPrintout>? TournamentResults { get; set; }

    public bool RecordScoreIsDisabled
    {
        get
        {
            return !SelectedTournament?.RegisteredTeams?.Any() ?? true;
        }
    }

    public bool DownloadResultIsDisabled
    {
        get
        {
            return RecordScoreIsDisabled || !(TournamentResults?.Any() ?? false);
        }
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SelectedTournament = await tournamentService.GetTournamentById(TournamentId!);
        TournamentResults = (await tournamentService.GetTournamentResultPrintout(TournamentId!))?.ToList() ?? [];
    }

    private async Task DownloadResultPrintout()
    {
        var binaryData = await tournamentService.DownloadResultsSpreadsheet(TournamentId!);
        var fileName = $"{DateTime.Now:yyyy-M-d}-{TournamentId}.xlsx";

        await jsRuntime.InvokeVoidAsync("saveFile", fileName, binaryData);
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