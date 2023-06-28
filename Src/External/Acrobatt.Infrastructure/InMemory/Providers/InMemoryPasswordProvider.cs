using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;

namespace Acrobatt.Infrastructure.InMemory.Providers;

public sealed class InMemoryPasswordProvider : IPasswordProvider
{
    public string Hash(Account account, string password)
    {
        return string.Concat(password, password);
    }

    public bool Verify(Account account, string plainPassword, string hashedPassword)
    {
        return hashedPassword.Contains(plainPassword);
    }
}