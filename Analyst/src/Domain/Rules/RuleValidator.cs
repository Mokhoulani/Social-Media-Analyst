using Domain.Shared;

namespace Domain.Rules;

public static class RuleValidator
{
    public static Result<T> Validate<T>(T value, params Rule[] rules)
    {
        foreach (var rule in rules)
        {
            if (rule.IsBroken())
                return Result.Failure<T>(rule.Error);
        }

        return Result.Success(value);
    }
}