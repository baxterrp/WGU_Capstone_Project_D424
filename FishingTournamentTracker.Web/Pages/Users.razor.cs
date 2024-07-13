using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class Users
{
    [CascadingParameter]
    public Admin? LoggedInAdministrator { get; set; }
    public List<Library.Models.DataModels.User>? SelectedUsers { get; set; }
    public string? CurrentUserId {  get; set; } = string.Empty;
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int? TotalPages { get; set; }
    public string? SearchValue {  get; set; }

    private void GoToUserPage(string userId)
    {
        navigationManager.NavigateTo($"/user/{userId}");
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Loading = true;

        if (string.IsNullOrWhiteSpace(LoggedInAdministrator?.Id))
        {
            navigationManager.NavigateTo("/");
        }

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
        finally
        {
            Loading = false;
        }
    }
    
    private async Task Delete()
    {
        Loading = true;
        await userService.DeleteUser(CurrentUserId!);
        await ResetData();
        Loading = false;
    }

    private async Task Search(int? page = null)
    {
        try
        {
            Loading = true;
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
        finally
        {
            Loading = false;
        }
    }

    private async Task ResetData()
    {
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

        await InvokeAsync(async () => StateHasChanged());
    }
}
