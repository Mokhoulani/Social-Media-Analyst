using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Password : ValueObject
{
    public string Hash { get; } 


    private Password() { }

    private Password(string hash)
    {
        Hash = hash;
    }
    
    public static Password Create(string plainTextPassword)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(plainTextPassword, workFactor: 12); 

        return new Password(hash);
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