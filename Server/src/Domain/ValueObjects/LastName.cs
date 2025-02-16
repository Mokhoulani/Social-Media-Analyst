using Domain.Primitives;


namespace Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static LastName Create(string lastName)
    {
        return new LastName(lastName);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}