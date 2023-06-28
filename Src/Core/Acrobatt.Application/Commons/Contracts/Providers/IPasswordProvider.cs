namespace Acrobatt.Application.Commons.Contracts.Providers;

public interface IPasswordProvider
{
    string Hash(Domain.Accounts.Account account, string password);
    bool Verify(Domain.Accounts.Account account, string plainPassword, string hashedPassword);
}