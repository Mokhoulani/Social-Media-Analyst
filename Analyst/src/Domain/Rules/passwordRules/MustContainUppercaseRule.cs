using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MustContainUppercaseRule(string password) : Rule
{
    public override bool IsBroken() => !password.Any(char.IsUpper);
    public override Error Error => Domain.Errors.DomainErrors.Password.MissingUpperCase;
}