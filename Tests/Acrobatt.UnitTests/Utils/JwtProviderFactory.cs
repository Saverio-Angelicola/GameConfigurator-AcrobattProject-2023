using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Infrastructure.InMemory.Providers;
using Acrobatt.Infrastructure.Providers;

namespace Acrobatt.UnitTests.Utils;

internal class JwtProviderFactory
{
    public static IJwtProvider GetJwtProvider()
    {
        return new InMemoryJWTProvider();
    }
}