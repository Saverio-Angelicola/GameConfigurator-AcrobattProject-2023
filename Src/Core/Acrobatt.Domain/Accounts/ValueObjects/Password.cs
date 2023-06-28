using System.Text.RegularExpressions;
using Acrobatt.Domain.Accounts.Exceptions;

namespace Acrobatt.Domain.Accounts.ValueObjects;

public sealed record Password(string Value)
{
    public static Password Init(string password)
    {
        Regex isGoodPasswordFormat = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        
        if (!isGoodPasswordFormat.IsMatch(password))
        {
            throw new PasswordInvalidFormatException();
        }

        return new Password(password);
    }
}