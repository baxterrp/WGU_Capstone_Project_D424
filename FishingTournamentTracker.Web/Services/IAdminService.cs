using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Web.Services;

public interface IAdminService
{
    Task<Admin?> Register(Admin admin);
    Task<Admin?> Login(Admin admin);
}
