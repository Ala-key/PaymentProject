using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity;

public interface IUserService
{
    /// <summary>
    /// Проверка валидности кредов пользователя.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> IsValidUserCredentialsAsync(string login, string password);

    /// <summary>
    /// Получение роли пользователя.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    Task<string> GetUserRoleAsync(string login);
    
    /// <summary>
    /// Запрос баланса.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<decimal> GetBalance(string username);
}

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<string> GetUserRoleAsync(string login)
    {
        var user = await userManager.FindByNameAsync(login);
        return (await userManager.GetRolesAsync(user)).First();
    }



    public async Task<decimal> GetBalance(string login)
    {
        var user = await userManager.FindByNameAsync(login);
        if (user is null)
            throw new InvalidOperationException("Такой пользователь не существует.");
        return user.Balance;
    }

    public async Task<bool> IsValidUserCredentialsAsync(string login, string password)
    {
        var user = await userManager.FindByNameAsync(login);
        return await userManager.CheckPasswordAsync(user, password);
    }
    
    
}