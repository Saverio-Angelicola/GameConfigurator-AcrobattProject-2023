using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Infrastructure.InMemory.Providers;

namespace Acrobatt.UnitTests.Utils;

internal class PasswordProviderFactory
{
    public static IPasswordProvider GetPasswordProvider()
    {
        return new InMemoryPasswordProvider();
    }
}