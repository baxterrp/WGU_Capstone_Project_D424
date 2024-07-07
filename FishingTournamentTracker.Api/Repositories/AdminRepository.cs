using Dapper;
using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.Extensions.Options;

namespace FishingTournamentTracker.Api.Repositories
{
    public class AdminRepository(IOptions<DatabaseConfiguration> databaseConfiguration) : BaseRepository(databaseConfiguration), IAdminRepository
    {
        public async Task<bool> CheckAdminExists(string userName)
        {
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.UserName), userName);

            var existingAdmin = await Search<Admin>(parameters);
            return existingAdmin is not null && existingAdmin.Any();
        }

        public async Task<Admin?> Login(Admin admin)
        {
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.UserName), admin.UserName);

            return (await Search<Admin>(parameters))?.FirstOrDefault();
        }

        public async Task<Admin?> RegisterNewAdmin(Admin admin)
        {
            return await Insert(admin);
        }
    }
}
