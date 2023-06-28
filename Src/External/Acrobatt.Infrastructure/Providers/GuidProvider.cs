using Acrobatt.Application.Commons.Contracts.Providers;

namespace Acrobatt.Infrastructure.Providers;

public class GuidProvider : IGuidProvider
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}