using Acrobatt.Application.Commons.Contracts.Providers;

namespace Acrobatt.Infrastructure.InMemory.Providers;

public sealed class InMemoryGuidProvider : IGuidProvider
{
    public Guid Generate()
    {
        return Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");
    }
}