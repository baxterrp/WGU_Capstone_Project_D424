using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Web.Constants;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class Home : BaseFormValidationPage<Admin>
{
    [CascadingParameter]
    public Action<Admin>? SetLoggedInAdmin { get; set; }

    public string? UserName {  get; set; }
    public string? Password { get; set; }
    public string? LoginErrorClass {  get; set; }

    private void NavigateToRegister()
    {
        navigationManager.NavigateTo("/register");
    }

    private async Task Login()
    {
        LoginErrorClass = string.Empty;
        InputValidationClasses![nameof(UserName)] = string.IsNullOrWhiteSpace(UserName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(Password)] = string.IsNullOrWhiteSpace(Password) ? ClassNames.InvalidField : string.Empty;

        if (CheckHasFormErrors()) { return; }

        var admin = await adminService.Login(new Admin
        {
            UserName = UserName,
            Password = Password
        });

        if (admin is null)
        {
            LoginErrorClass = ClassNames.InvalidField;
            return;
        }

        SetLoggedInAdmin?.Invoke(admin);

        navigationManager.NavigateTo("/users");
    }
}
