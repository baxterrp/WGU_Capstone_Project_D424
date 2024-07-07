using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Web.Constants;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class Register : BaseFormValidationPage<Admin>
{
    [CascadingParameter]
    public Action<Admin>? SetLoggedInAdmin { get; set; }
    private string? RegisterErrorClass { get; set; }
    private string? UserName { get; set; }
    private string? Password { get; set; }

    private async Task OnSubmit()
    {
        Loading = true;

        // I couldnt figure out why my loader wouldn't display until after an additional async operation has executed,
        // waiting 1 millisecond along with changing state to get icon 
        await Task.WhenAll(InvokeAsync(() => StateHasChanged()), Task.Delay(1));

        RegisterErrorClass = string.Empty;
        InputValidationClasses![nameof(UserName)] = string.IsNullOrWhiteSpace(UserName) ? ClassNames.InvalidField : string.Empty;
        InputValidationClasses![nameof(Password)] = string.IsNullOrWhiteSpace(Password) ? ClassNames.InvalidField : string.Empty;

        if (CheckHasFormErrors()) { return; }

        var admin = await adminService.Register(new Admin
        {
            UserName = UserName,
            Password = Password,
        });

        Loading = false;
        await InvokeAsync(() => StateHasChanged());

        if (admin is null)
        {
            RegisterErrorClass = ClassNames.InvalidField;
            return;
        }

        SetLoggedInAdmin?.Invoke(admin);
        navigationManager.NavigateTo("/users");
    }

    private async Task Do()
    {


    }
}
