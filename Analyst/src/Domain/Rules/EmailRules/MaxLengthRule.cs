using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.EmailRules;

public class MaxLengthRule(string value, int maxLength) : Rule
{
    public override bool IsBroken() => value.Length > maxLength;

    public override Error Error => DomainErrors.Email.TooLong;
}