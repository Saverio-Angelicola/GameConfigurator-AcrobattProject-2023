namespace Acrobatt.Application.Accounts.Exceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException() : base("The password is incorrect")
    {
    }
}