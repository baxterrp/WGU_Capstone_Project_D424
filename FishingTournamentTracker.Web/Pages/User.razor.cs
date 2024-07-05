using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class User
{
    [Parameter]
    public string? UserId { get; set; }

    public Library.Models.DataModels.User? SelectedUser { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SelectedUser = await userService.GetUserById(UserId!);
    }

    public async Task UpdateUser()
    { 
        SelectedUser = await userService.EditUser(SelectedUser!);
        navigationManager.NavigateTo("/users");
    }
}
