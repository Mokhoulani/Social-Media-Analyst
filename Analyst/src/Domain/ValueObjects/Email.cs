using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;


namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private const int MaxLength = 100;
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }
    
    public static Result<Email> Create(string email) =>
        Result.Create(email)
            .Ensure(
                e => !string.IsNullOrWhiteSpace(e),
                DomainErrors.Email.Empty)
            .Ensure(
                e => e.Length <= MaxLength,
                DomainErrors.Email.TooLong)
            .Ensure(
                e => Regex.IsMatch(e, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
                DomainErrors.Email.InvalidFormat)
            .Map(e => new Email(e));
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
