using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Api.Services
{
    public interface IAdminService
    {
        Task<Admin?> RegisterNewAdmin(Admin admin);
        Task<Admin> Login(Admin admin);
    }
}
