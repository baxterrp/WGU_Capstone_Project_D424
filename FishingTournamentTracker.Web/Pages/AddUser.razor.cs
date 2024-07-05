namespace FishingTournamentTracker.Web.Pages;

public partial class AddUser()
{
    public Library.Models.DataModels.User? SelectedUser { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SelectedUser = new();
    }

    private async Task OnSubmit()
    {
        await userService.AddUser(SelectedUser!);
        SelectedUser = new();
        navigationManager.NavigateTo("/users");
    }
}