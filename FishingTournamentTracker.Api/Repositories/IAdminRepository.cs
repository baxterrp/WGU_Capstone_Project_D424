using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Api.Repositories
{
    public interface IAdminRepository
    {
        Task<bool> CheckAdminExists(string userName);
        Task<Admin?> RegisterNewAdmin(Admin admin);
        Task<Admin?> Login(Admin admin);
    }
}
