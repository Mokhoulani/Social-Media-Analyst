using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MustContainLowercaseRule(string value) : Rule
{
    public override bool IsBroken() => !value.Any(char.IsLower);

    public override Error Error => DomainErrors.Password.MissingLowerCase;
}