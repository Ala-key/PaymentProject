using Core.Models;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity;

public interface IUserCreator
{
    Task<User> AddNewUserAsync(string login, string password);
}

public class UserCreator(UserManager<User> userManager) : IUserCreator
{
    public async Task<User> AddNewUserAsync(string login, string password)
    {
        
        var user = new User(login);
        var userResult = await userManager.CreateAsync(user, password);
        CheckOperationResult(userResult, "Пользователь не создан!");
        var roleResult = await userManager.AddToRoleAsync(user, DefaultRoles.User);
        CheckOperationResult(roleResult, "Роль пользователя не применена");
        return user;
        
    }

    private void CheckOperationResult(IdentityResult result, string message)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(message);
        }
    }
}