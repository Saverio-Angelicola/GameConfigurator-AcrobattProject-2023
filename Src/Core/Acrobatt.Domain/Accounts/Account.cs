using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Exceptions;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Commons;

namespace Acrobatt.Domain.Accounts;

public sealed class Account : Aggregate<Guid>
{
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Pseudo { get; private set; }
    
    public string Email { get; private set; }
    
    public Password Password { get; private set; }

    public List<GameConfiguration> GameConfigurations { get; private set; }

    public Account(Guid id, string firstName, string lastName, string pseudo, string email, Password password): base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Pseudo = pseudo;
        Email = email;
        Password = password;
        GameConfigurations = new List<GameConfiguration>();
    }

    public static Account Create(Guid id, string firstName, string lastName, string pseudo, string email, string password)
    {
        return new Account(id, firstName, lastName, pseudo, email, Password.Init(password));
    }

    public void UpdatePassword(string password)
    {
        Password = new Password(password);
    }

    public void UpdateInformations(string firstName, string lastName, string pseudo, string email)
    {
        FirstName = firstName?? FirstName;
        LastName = lastName ?? LastName;
        Pseudo = pseudo ?? Pseudo;
        Email = email ?? Email;
    }

    public void AddGameConfiguration(GameConfiguration gameConfiguration)
    {
        GameConfigurations.Add(gameConfiguration);
    }
}