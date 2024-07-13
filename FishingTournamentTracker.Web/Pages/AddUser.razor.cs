using FishingTournamentTracker.Web.Constants;

namespace FishingTournamentTracker.Web.Pages;

using UserModel = Library.Models.DataModels.User;

public partial class AddUser : BaseFishingTournamentView<UserModel>
{
    public UserModel? SelectedUser { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SelectedUser = new();
    }

    private async Task OnSubmit()
    {
        InputValidationClasses![nameof(UserModel.FirstName)] = string.IsNullOrWhiteSpace(SelectedUser?.FirstName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.LastName)] = string.IsNullOrWhiteSpace(SelectedUser?.LastName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.Grade)] = SelectedUser?.Grade is null ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.Birthday)] = SelectedUser?.Birthday is null ? ClassNames.InvalidField : string.Empty;
        
        Loading = true;

        if (CheckHasFormErrors()) { return; }

        await userService.AddUser(SelectedUser!);
        SelectedUser = new();
        Loading = false;
        navigationManager.NavigateTo("/users");
    }
}