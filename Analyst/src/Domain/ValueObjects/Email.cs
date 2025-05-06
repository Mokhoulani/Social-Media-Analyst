using Domain.Primitives;
using Domain.Rules;
using Domain.Rules.EmailRules;
using Domain.Shared;
using Domain.Shared.Extensions;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private const int MaxLength = 100;
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        return RuleValidator
            .Validate(email, new NotEmptyRule(email), new MaxLengthRule(email, MaxLength), new EmailFormatRule(email))
            .Map(e => new Email(e));
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}