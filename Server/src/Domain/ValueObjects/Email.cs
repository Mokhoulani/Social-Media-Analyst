using Domain.Primitives;


namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

   
    public static Email Create(string email)
    {
        return new Email(email);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
