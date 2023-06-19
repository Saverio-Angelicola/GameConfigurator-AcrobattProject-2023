using Acrobatt.Domain.Accounts;
using Acrobatt.Infrastructure.InMemory.Repositories;

namespace Acrobatt.UnitTests.Utils;

internal class AccountRepositoryFactory
{
    public static IAccountRepository GetAccountRepository()
    {
        return new InMemoryAccountRepository();
    }
}