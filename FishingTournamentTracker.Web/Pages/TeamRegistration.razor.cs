using FishingTournamentTracker.Library.Models.DataModels;
using FishingTournamentTracker.Library.Models.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public partial class TeamRegistration
{
    [Parameter]
    public string? TournamentId { get; set; }

    public TournamentViewModel? Tournament { get; set; }

    public List<Library.Models.DataModels.User>? Users { get; set; }

    public bool RegisterIsDisabled
    {
        get
        {
            return string.IsNullOrWhiteSpace(TournamentRegistration?.UserOne) || string.IsNullOrWhiteSpace(TournamentRegistration?.UserTwo);
        }
    }

    public TournamentRegistration? TournamentRegistration { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Loading = true;

        TournamentRegistration = new()
        {
            Tournament = TournamentId,
        };
        Tournament = await tournamentService.GetTournamentById(TournamentId!);
        var users = await userService.GetAllUsers();
        Users = users is not null ? [.. users] : [];

        Loading = false;
    }

    private async Task OnSubmit()
    {
        Loading = true;
        await tournamentService.RegisterTeam(TournamentRegistration!);
        Loading = false;

        navigatonManager.NavigateTo($"/tournament/{TournamentId}");
    }

}
