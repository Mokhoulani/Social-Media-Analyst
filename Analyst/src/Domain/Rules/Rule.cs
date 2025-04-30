
using Domain.Shared;

namespace Domain.Rules;

public abstract class Rule
{
    public abstract bool IsBroken();
    public abstract Error Error { get; }
}