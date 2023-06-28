namespace Acrobatt.Domain.Commons;

public class Entity<TId>
{
    public TId Id { get; set; }

    protected Entity(TId id)
    {
        Id = id;
    }
}