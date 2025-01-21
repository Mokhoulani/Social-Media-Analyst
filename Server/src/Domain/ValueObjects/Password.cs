using Domain.Primitives;


namespace Domain.ValueObjects;

public sealed class Password : ValueObject
{
    private Password(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Password Create(string password)
    {
        return new Password(password);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
