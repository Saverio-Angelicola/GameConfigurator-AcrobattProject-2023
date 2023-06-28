namespace Acrobatt.Domain.Accounts.Exceptions;

public class PasswordInvalidFormatException : Exception
{
    public PasswordInvalidFormatException() : base("The password must have at least 8 characters with a number, a capital letter and a special character")
    {
        
    }
}