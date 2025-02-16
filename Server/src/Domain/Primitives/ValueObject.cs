using System.Collections.Generic;
using System.Linq;

namespace Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetAtomicValues();

    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var value in GetAtomicValues())
        {
            hash.Add(value);
        }
        return hash.ToHashCode();
    }

    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }
}
