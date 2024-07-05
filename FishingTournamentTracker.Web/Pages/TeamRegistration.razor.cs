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

        TournamentRegistration = new()
        {
            Tournament = TournamentId,
        };
        Tournament = await tournamentService.GetTournamentById(TournamentId!);
        var users = await userService.GetUsers(string.Empty, string.Empty);
        Users = users is not null ? [.. users] : [];
    }

    private async Task OnSubmit()
    {
        await tournamentService.RegisterTeam(TournamentRegistration!);
        navigatonManager.NavigateTo($"/tournament/{TournamentId}");
    }

}
