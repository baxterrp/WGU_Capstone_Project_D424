using FishingTournamentTracker.Web.Constants;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

using UserModel =  Library.Models.DataModels.User;

public partial class User : BaseFishingTournamentView<UserModel>
{
    [Parameter]
    public string? UserId { get; set; }

    public UserModel? SelectedUser { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Loading = true;
        SelectedUser = await userService.GetUserById(UserId!);
        Loading = false;
    }

    public async Task UpdateUser()
    {
        Loading = true;
        InputValidationClasses![nameof(UserModel.FirstName)] = string.IsNullOrWhiteSpace(SelectedUser?.FirstName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.LastName)] = string.IsNullOrWhiteSpace(SelectedUser?.LastName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.Grade)] = SelectedUser?.Grade is null ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(UserModel.Birthday)] = SelectedUser?.Birthday is null ? ClassNames.InvalidField : string.Empty;

        if (CheckHasFormErrors()) { return; }

        SelectedUser = await userService.EditUser(SelectedUser!);

        Loading = false;

        navigationManager.NavigateTo("/users");
    }
}
