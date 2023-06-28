using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Infrastructure.InMemory.Providers;

namespace Acrobatt.UnitTests.Utils;

internal class GuidProviderFactory
{
    public static IGuidProvider GetGuidProvider()
    {
        return new InMemoryGuidProvider();
    }
}