using Domain.Primitives;


namespace Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static FirstName Create(string firstName)
    {
     
        return new FirstName(firstName);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}