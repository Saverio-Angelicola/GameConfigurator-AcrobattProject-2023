namespace Acrobatt.Application.Accounts.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base("Account does not exist") {}
}