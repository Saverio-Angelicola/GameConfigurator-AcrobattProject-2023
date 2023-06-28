using Acrobatt.Domain.Commons;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acrobatt.Infrastructure.Persistence.Repositories.write;

public class BaseRepository<TAggregate, TId>: IBaseRepository<TAggregate, TId> where TAggregate : Aggregate<TId>
{
    protected readonly PostgresContext Context;

    protected BaseRepository(PostgresContext context)
    {
        Context = context;
    }

    public async Task<List<TAggregate>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<TAggregate>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken)
    {
        return await Context.Set<TAggregate>().FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(TAggregate entity, CancellationToken cancellationToken)
    {
        await Context.Set<TAggregate>().AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken)
    {
        Context.Set<TAggregate>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken)
    {
        Context.Set<TAggregate>().Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }
}