namespace PaymentProject.Response;

public class BalanceResponse(decimal balance)
{
    public decimal Balance => balance;
}