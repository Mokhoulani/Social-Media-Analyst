using Domain.Primitives;
using Domain.Rules;
using Domain.Rules.passwordRules;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class Password : ValueObject
{
    private const int MinLength = 8;
    private const int MaxLength = 50;
    public string Hash { get; } 
    
    private Password() { }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Result<Password> Create(string plainTextPassword)
    {
        return RuleValidator
            .Validate(plainTextPassword,
            [
            new NotEmptyPasswordRule(plainTextPassword),
            new MinLengthPasswordRule(plainTextPassword, MinLength),
            new MaxLengthPasswordRule(plainTextPassword, MaxLength),
            new MustContainUppercaseRule(plainTextPassword),
            new MustContainLowercaseRule(plainTextPassword),
            new MustContainDigitRule(plainTextPassword),
            new MustContainSpecialCharRule(plainTextPassword)
            ])
            .Map(p => new Password(BCrypt.Net.BCrypt.HashPassword(p, workFactor: 12)));
    }


    public bool Verify(string plainTextPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainTextPassword, Hash);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Hash;
    }
}