using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MustContainDigitRule(string value) : Rule
{
    public override bool IsBroken() => !value.Any(char.IsDigit);

    public override Error Error => DomainErrors.Password.MissingDigit;
}