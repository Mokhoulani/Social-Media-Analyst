using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.EmailRules;

public class NotEmptyRule(string value) : Rule
{
    public override bool IsBroken() => string.IsNullOrWhiteSpace(value);

    public override Error Error => DomainErrors.Email.Empty;
}