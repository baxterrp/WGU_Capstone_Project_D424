using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

using UserModel =  Library.Models.DataModels.User;

public partial class User : BaseFormValidationPage<UserModel>
{
    [Parameter]
    public string? UserId { get; set; }

    public UserModel? SelectedUser { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SelectedUser = await userService.GetUserById(UserId!);
    }

    public async Task UpdateUser()
    {
        InputValidationClasses![nameof(UserModel.FirstName)] = string.IsNullOrWhiteSpace(SelectedUser?.FirstName) ? "is-invalid" : string.Empty;
        InputValidationClasses![nameof(UserModel.LastName)] = string.IsNullOrWhiteSpace(SelectedUser?.LastName) ? "is-invalid" : string.Empty;
        InputValidationClasses![nameof(UserModel.Grade)] = SelectedUser?.Grade is null ? "is-invalid" : string.Empty;
        InputValidationClasses![nameof(UserModel.Birthday)] = SelectedUser?.Birthday is null ? "is-invalid" : string.Empty;

        if (CheckHasFormErrors()) { return; }

        SelectedUser = await userService.EditUser(SelectedUser!);
        navigationManager.NavigateTo("/users");
    }
}
