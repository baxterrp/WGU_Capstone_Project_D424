using FishingTournamentTracker.Library.Models.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class RecordTournamentScore
{
    [Parameter]
    public string? TournamentId { get; set; }

    public TeamScoreViewModel? TeamScore { get; set; }

    public string? SelectedTeamId {  get; set; }

    public TournamentViewModel? SelectedTournament { get; set; }

    public double? FishWeight { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Loading = true;

        FishWeight = 0;

        TeamScore = new()
        {
            TournamentId = TournamentId,
            Fish = []
        };

        SelectedTournament = await tournamentService.GetTournamentById(TournamentId!);
        Loading = false;
    }

    private void AddFish(EventArgs e)
    {
        TeamScore!.Fish.Add(new RecordedFishViewModel
        {
            Weight = FishWeight,
        });

        FishWeight = 0;

        StateHasChanged();
    }

    private async Task OnSubmit(EventArgs e)
    {
        Loading = true;
        TeamScore!.TeamId = SelectedTeamId;
        await tournamentService.SaveTeamScore(TeamScore!);
        Loading = false;
        navigationManager.NavigateTo($"/tournament/{TournamentId}");
    }
}
