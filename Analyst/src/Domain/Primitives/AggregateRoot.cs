namespace Domain.Primitives;

public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TKey id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}