using DataLayer.Models;

namespace PaymentProject.Response;

public class UserResponse (User user)
{
    public string Username  => user.UserName;
}