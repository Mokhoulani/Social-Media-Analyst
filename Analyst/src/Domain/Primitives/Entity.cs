namespace Domain.Primitives;

public abstract class Entity<TKey>(TKey id) : IEquatable<Entity<TKey>> where TKey : notnull
{
    protected Entity() : this(default!)
    {
    }

    public TKey Id { get; private init; } = id;

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

    public override bool Equals(object? obj) => obj is Entity<TKey> entity && Equals(entity);
    public override int GetHashCode() => HashCode.Combine(Id, GetType());
}