using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class NotEmptyPasswordRule(string value) : Rule
{
    public override bool IsBroken() => string.IsNullOrWhiteSpace(value);

    public override Error Error => DomainErrors.Password.Empty;
}