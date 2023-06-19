namespace Acrobatt.Application.Accounts.Features.AccountDetails;

public class AccountDetailsViewModel
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Pseudo { get; private set; }
    public string Email { get; private set; }

    public AccountDetailsViewModel(Guid id, string firstName, string lastName, string pseudo, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Pseudo = pseudo;
        Email = email;
    }
};