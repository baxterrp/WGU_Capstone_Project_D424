using FishingTournamentTracker.Web.Constants;

namespace FishingTournamentTracker.Web.Pages;

using TournamentModel = Library.Models.DataModels.Tournament;

public partial class AddTournament : BaseFishingTournamentView<TournamentModel>
{
    public TournamentModel? SelectedTournament { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SelectedTournament ??= new();
    }

    private async Task OnSubmit()
    {
        InputValidationClasses![nameof(TournamentModel.Name)] = string.IsNullOrWhiteSpace(SelectedTournament?.Name) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(TournamentModel.Lake)] = string.IsNullOrWhiteSpace(SelectedTournament?.Lake) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(TournamentModel.Date)] = SelectedTournament?.Date is null ? ClassNames.InvalidField : string.Empty;

        Loading = true;

        if (CheckHasFormErrors()) { return; }

        await tournamentService.CreateTournament(SelectedTournament!);
        Loading = false;
        navigationManager.NavigateTo("/tournaments");
    }
}
