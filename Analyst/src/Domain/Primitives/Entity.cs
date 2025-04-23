namespace Domain.Primitives;

public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
{
    protected Entity(TKey id) => Id = id;

    protected Entity() => Id = default!;

    public TKey Id { get; private init; }

    public static bool operator ==(Entity<TKey>? first, Entity<TKey>? second)
    {
        if (first is null && second is null) return true;
        if (first is null || second is null) return false;
        return first.Equals(second);
    }

    public static bool operator !=(Entity<TKey>? first, Entity<TKey>? second) => !(first == second);

    public bool Equals(Entity<TKey>? other)
{
    if (ReferenceEquals(this, other)) return true;
    if (other is null || other.GetType() != GetType()) return false;
    
    return EqualityComparer<TKey>.Default.Equals(other.Id, Id);
}


    public override bool Equals(object? obj) =>
        obj is Entity<TKey> entity && Equals(entity);

    public override int GetHashCode() => HashCode.Combine(Id, GetType());
}
