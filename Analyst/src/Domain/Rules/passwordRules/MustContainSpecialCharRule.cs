using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MustContainSpecialCharRule(string value) : Rule
{
    public override bool IsBroken() => !value.Any(char.IsSymbol);

    public override Error Error => DomainErrors.Password.MissingSpecialChar;
}