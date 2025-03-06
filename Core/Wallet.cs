using DataLayer.Data;
using DataLayer.Models;

namespace Core;

public class Wallet(User user)
{
    private const decimal Cost = 1.1m;
    
    public Payment Pay()
    {
        if (user.Balance < Cost)
            throw new InvalidOperationException("У вас недостаточно денег для оплаты!");
        
        user.Balance -= 1.1m;
        return new Payment()
        {
            User = user,
            Sum = Cost,
            Timestamp = DateTime.UtcNow
        };
    }
}