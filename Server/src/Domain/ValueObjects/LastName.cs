using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;


namespace Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<LastName> Create(string lastName) =>
        Result.Create(lastName)
            .Ensure(
                l => !string.IsNullOrWhiteSpace(l),
                DomainErrors.LastName.Empty)
            .Ensure(
                l => l.Length <= MaxLength,
                DomainErrors.LastName.TooLong)
            .Map(l => new LastName(l));
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}