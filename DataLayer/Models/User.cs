using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public sealed class User : IdentityUser
    {
        public User() { }
        
        public User(string login)
        {
            UserName = login;
            Balance = 8;
        }
        
        /// <summary>
        /// Баланс пользователя в долларах
        /// </summary>
        public decimal Balance { get; set; }
        
        /// <summary>
        /// Платежи пользователя.
        /// </summary>
        public List<Payment> Payments { get; set; } = new();
    }
}
