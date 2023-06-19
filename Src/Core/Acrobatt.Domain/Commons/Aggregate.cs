namespace Acrobatt.Domain.Commons;

public abstract class Aggregate<TId> : Entity<TId>
{
    protected Aggregate(TId id) : base(id)
    {
    }
}