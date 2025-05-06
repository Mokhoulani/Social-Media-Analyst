using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;
using Domain.Shared.Extensions;


namespace Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FirstName> Create(string firstName) =>
        Result.Create(firstName)
            .Ensure(
                f => !string.IsNullOrWhiteSpace(f),
                DomainErrors.FirstName.Empty)
            .Ensure(
                f => f.Length <= MaxLength,
                DomainErrors.FirstName.TooLong)
            .Map(f => new FirstName(f));
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}