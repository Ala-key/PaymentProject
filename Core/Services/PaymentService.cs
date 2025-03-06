using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public interface IPaymentService
    {
        /// <summary>
        /// Совершить оплату.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task PayAsync(User user);
        
        /// <summary>
        /// Посмотреть все платежи пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<Payment>> GetUserPaymentsAsync(User user);
    }

    public class PaymentService(AppDbContext context) : IPaymentService
    {
        public async Task PayAsync(User user)
        {
            var wallet = new Wallet(user);
            var payment = wallet.Pay();
            await context.Payments.AddAsync(payment);
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(User user) =>
            await context.Payments
                .Include(p => p.User)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
         
    }
}
