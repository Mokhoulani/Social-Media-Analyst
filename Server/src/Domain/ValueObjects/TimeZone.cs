using Domain.Base;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class TimeZone : BaseValueObject
{
    public string Value { get; }

    private TimeZone() { }
    
    public TimeZone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Time zone cannot be empty.");
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}