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
        Loading = true;

        // I couldnt figure out why my loader wouldn't display until after an additional async operation has executed,
        // waiting 1 millisecond along with changing state to get icon 
        await Task.WhenAll(InvokeAsync(() => StateHasChanged()), Task.Delay(1));

        LoginErrorClass = string.Empty;
        InputValidationClasses![nameof(UserName)] = string.IsNullOrWhiteSpace(UserName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(Password)] = string.IsNullOrWhiteSpace(Password) ? ClassNames.InvalidField : string.Empty;

        if (CheckHasFormErrors()) { return; }

        var admin = await adminService.Login(new Admin
        {
            UserName = UserName,
            Password = Password
        });

        Loading = false;

        if (admin is null)
        {
            LoginErrorClass = ClassNames.InvalidField;
            return;
        }

        SetLoggedInAdmin?.Invoke(admin);
        navigationManager.NavigateTo("/users");
    }
}
