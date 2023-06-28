using Acrobatt.Infrastructure.Persistence.Contexts;

namespace Acrobatt.Infrastructure.Persistence.Repositories.Read;

public abstract class BaseReadRepository
{
    protected readonly PostgresContext Context;

    protected BaseReadRepository(PostgresContext context)
    {
        Context = context;
    }
}