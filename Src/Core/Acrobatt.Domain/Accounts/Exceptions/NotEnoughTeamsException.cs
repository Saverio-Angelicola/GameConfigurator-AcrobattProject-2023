namespace Acrobatt.Domain.Accounts.Exceptions;

public class NotEnoughTeamsException : Exception
{
    public NotEnoughTeamsException() : base("There are not enough teams: There must be at least two teams")
    {
    }
}