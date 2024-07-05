namespace FishingTournamentTracker.Web.Pages;

public partial class Users
{
    public List<Library.Models.DataModels.User>? SelectedUsers { get; set; }

    public void GoToUserPage(string userId)
    {
        navigationManager.NavigateTo($"/user/{userId}");
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            var users = await userService.GetUsers(string.Empty, string.Empty);
            SelectedUsers = users?.ToList() ?? [];
        }
        catch
        {
            SelectedUsers = [];
        }
    }
}
