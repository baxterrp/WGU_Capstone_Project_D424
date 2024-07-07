using FishingTournamentTracker.Api.Exeptions;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Api.Services;

public class AdminService(IAdminRepository adminRepository) : IAdminService
{
    public async Task<Admin> Login(Admin admin)
    {
        ArgumentNullException.ThrowIfNull(admin, nameof(admin));
        ArgumentException.ThrowIfNullOrWhiteSpace(admin.UserName, nameof(Admin.UserName));
        ArgumentException.ThrowIfNullOrWhiteSpace(admin.Password, nameof(Admin.Password));

        return await adminRepository.Login(admin) ?? throw new UnauthorizedLoginAttemptException();
    }

    public async Task<Admin?> RegisterNewAdmin(Admin admin)
    {
        ArgumentNullException.ThrowIfNull(admin, nameof(admin));
        ArgumentException.ThrowIfNullOrWhiteSpace(admin.UserName, nameof(Admin.UserName));

        if (await adminRepository.CheckAdminExists(admin.UserName))
        {
            throw new ArgumentException($"User name {admin.UserName} is already taken");
        }

        return await adminRepository.RegisterNewAdmin(admin);
    }
}
