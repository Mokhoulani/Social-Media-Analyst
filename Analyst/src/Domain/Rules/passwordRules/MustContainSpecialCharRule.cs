using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.passwordRules;

public class MustContainSpecialCharRule(string value) : Rule
{
    public override bool IsBroken() => !Regex.IsMatch(value, @"[!@#$%^&*(),.?""{}|<>]");

    public override Error Error => DomainErrors.Password.MissingSpecialChar;
}