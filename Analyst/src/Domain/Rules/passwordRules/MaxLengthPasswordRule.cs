using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MaxLengthPasswordRule(string password, int minLength) : Rule
{
    public override bool IsBroken() => password.Length < minLength;

    public override Error Error => DomainErrors.Password.TooLong;
}