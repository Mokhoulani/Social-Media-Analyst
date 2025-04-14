using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MaxLengthPasswordRule(string password, int maxLength) : Rule
{
    public override bool IsBroken() => password.Length > maxLength;

    public override Error Error => DomainErrors.Password.TooLong;
}