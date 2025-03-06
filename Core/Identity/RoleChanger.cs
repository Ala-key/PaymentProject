using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity;

public interface IRoleChanger
{
    Task<IdentityResult> ChangeAsync(string login, string roleName);
}

public class RoleChanger(UserManager<User> userManager) : IRoleChanger
{
    public async Task<IdentityResult> ChangeAsync(string login, string roleName)
    {
        var user = await userManager.FindByNameAsync(login);
        await RemoveLastRoles(user);
        return await AddNewRole(user, roleName);
    }

    private async Task RemoveLastRoles(User user)
    {
        var oldRoles = await userManager.GetRolesAsync(user);
        oldRoles.ToList()
            .ForEach(r => RemoveRoleForUser(user, r).Wait());
    }

    private async Task RemoveRoleForUser(User user, string oldRole)
    {
        await userManager.RemoveFromRoleAsync(user, oldRole);
    }

    private async Task<IdentityResult> AddNewRole(User user, string newRole)
    {
        var roles = new List<string>() { newRole };
        return await userManager.AddToRolesAsync(user, roles);
    }
}