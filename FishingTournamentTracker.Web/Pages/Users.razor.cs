namespace FishingTournamentTracker.Web.Pages;

public partial class Users
{
    public List<Library.Models.DataModels.User>? SelectedUsers { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int? TotalPages { get; set; }
    public string? SearchValue {  get; set; }

    public void GoToUserPage(string userId)
    {
        navigationManager.NavigateTo($"/user/{userId}");
    }

    private async Task Search(int? page = null)
    {
        try
        {
            var pageNumber = page ?? 1;
            var paginatedResponse = await userService.FilterUsers(SearchValue, string.Empty, pageNumber, PageSize);
            PageNumber = paginatedResponse?.PageNumber ?? 1;
            PageSize = paginatedResponse?.PageSize ?? 10;
            SelectedUsers = paginatedResponse?.Data?.ToList() ?? [];
            TotalPages = paginatedResponse?.TotalPages ?? 0;
        }
        catch
        {
            SelectedUsers = [];
        }
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            var paginatedResponse = await userService.FilterUsers(string.Empty, string.Empty, 1, 10);
            PageNumber = paginatedResponse?.PageNumber ?? 1;
            PageSize = paginatedResponse?.PageSize ?? 10;
            SelectedUsers = paginatedResponse?.Data?.ToList() ?? [];
            TotalPages = paginatedResponse?.TotalPages ?? 0;
        }
        catch
        {
            SelectedUsers = [];
        }
    }
}
