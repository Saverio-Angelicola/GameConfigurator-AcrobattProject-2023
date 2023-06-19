using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Acrobatt.Infrastructure.Providers;

public class PasswordProvider : IPasswordProvider
{
    private readonly IPasswordHasher<Account> _passwordHasher;

    public PasswordProvider(IPasswordHasher<Account> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Hash(Account account, string password)
    {
        return _passwordHasher.HashPassword(account, password);
    }

    public bool Verify(Account account, string plainPassword, string hashedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(account, hashedPassword, plainPassword) == PasswordVerificationResult.Success;
    }
}