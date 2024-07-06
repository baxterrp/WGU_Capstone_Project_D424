﻿namespace FishingTournamentTracker.Web.Pages;

using TournamentModel = Library.Models.DataModels.Tournament;

public partial class AddTournament : BaseFormValidationPage<TournamentModel>
{
    public TournamentModel? SelectedTournament { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SelectedTournament ??= new();
    }

    private async Task OnSubmit()
    {
        InputValidationClasses![nameof(TournamentModel.Name)] = string.IsNullOrWhiteSpace(SelectedTournament?.Name) ? "is-invalid" : string.Empty;
        InputValidationClasses![nameof(TournamentModel.Lake)] = string.IsNullOrWhiteSpace(SelectedTournament?.Lake) ? "is-invalid" : string.Empty;
        InputValidationClasses![nameof(TournamentModel.Date)] = SelectedTournament?.Date is null ? "is-invalid" : string.Empty;

        if (CheckHasFormErrors()) { return; }

        await tournamentService.CreateTournament(SelectedTournament!);
        navigationManager.NavigateTo("/tournaments");
    }
}
