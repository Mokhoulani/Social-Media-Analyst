using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Shared;

namespace Domain.Rules.EmailRules;

public class EmailFormatRule(string email) : Rule
{
    public override bool IsBroken() =>
        !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public override Error Error => DomainErrors.Email.InvalidFormat;
}