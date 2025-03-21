using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
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
        return Result.Create(plainTextPassword)
            .Ensure(
                p => !string.IsNullOrWhiteSpace(p),
                DomainErrors.Password.Empty)
            .Ensure(
                p => p.Length >= MinLength,
                DomainErrors.Password.TooShort)
            .Ensure(
                p => p.Length <= MaxLength,
                DomainErrors.Password.TooLong)
            .Ensure(
                p => Regex.IsMatch(p, @"[A-Z]"),
                DomainErrors.Password.MissingUpperCase)
            .Ensure(
                p => Regex.IsMatch(p, @"[a-z]"),
                DomainErrors.Password.MissingLowerCase)
            .Ensure(
                p => Regex.IsMatch(p, @"\d"),
                DomainErrors.Password.MissingDigit)
            .Ensure(
                p => Regex.IsMatch(p, @"[\W_]"),
                DomainErrors.Password.MissingSpecialChar)
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