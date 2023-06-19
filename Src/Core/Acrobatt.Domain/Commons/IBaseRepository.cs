namespace Acrobatt.Domain.Commons;

public interface IBaseRepository<TAggregate, TId> where TAggregate : Aggregate<TId>
{
    Task<List<TAggregate>> GetAllAsync(CancellationToken cancellationToken);

    Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken);

    Task AddAsync(TAggregate entity, CancellationToken cancellationToken);

    Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken);
}