using Core.Models;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.DbSeeders
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
    
    public class DefaultRolesDbLoader(RoleManager<IdentityRole> manager) : IDataSeeder
    {
        public async Task SeedAsync()
        {
            var newRoles = FindNewRoles();
            await WriteNewRolesAsync(newRoles);
        }

        IEnumerable<string> FindNewRoles()
        {
            var existingRoles = manager.Roles.Select(role => role.Name);
            return DefaultRoles
                .All()
                .Except(existingRoles);
        }

        private async Task WriteNewRolesAsync(IEnumerable<string> newRoles)
        {
            foreach (var name in newRoles)
            {
                await manager.CreateAsync(new IdentityRole(name));
            }
        }
    }
}
