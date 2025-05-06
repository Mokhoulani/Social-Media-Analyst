namespace Domain.Shared.Extensions;

public static class MatchExtensions
{
    public static Result<T> Match<T>(this Result<T> result, Func<T, Result<T>> onSuccess,
        Func<Error, Result<T>> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, Result<T>> onSuccess, Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : Result.Success(onFailure(result.Error));
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> onSuccess, Func<Error, T> onFailure)
    {
        return result.IsSuccess ? Result.Success(onSuccess(result.Value)) : Result.Success(onFailure(result.Error));
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> onSuccess, Func<Error, Result<T>> onFailure)
    {
        return result.IsSuccess ? Result.Success(onSuccess(result.Value)) : onFailure(result.Error);
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> onSuccess, Func<Error, T> onFailure,
        Func<T, Error, T> onBoth)
    {
        return result.IsSuccess
            ? Result.Success(onSuccess(result.Value))
            : Result.Success(onBoth(result.Value, result.Error));
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> onSuccess, Func<Error, T> onFailure,
        Func<T, Error, Result<T>> onBoth)
    {
        return result.IsSuccess ? Result.Success(onSuccess(result.Value)) : onBoth(result.Value, result.Error);
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<Result<T>>> onSuccess,
        Func<Error, Task<Result<T>>> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value).Result : onFailure(result.Error).Result;
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<Result<T>>> onSuccess,
        Func<Error, Task<T>> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value).Result : Result.Success(onFailure(result.Error).Result);
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<T>> onSuccess,
        Func<Error, Task<T>> onFailure)
    {
        return result.IsSuccess
            ? Result.Success(onSuccess(result.Value).Result)
            : Result.Success(onFailure(result.Error).Result);
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<T>> onSuccess,
        Func<Error, Task<Result<T>>> onFailure)
    {
        return result.IsSuccess ? Result.Success(onSuccess(result.Value).Result) : onFailure(result.Error).Result;
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<T>> onSuccess,
        Func<Error, Task<T>> onFailure, Func<T, Error, Task<T>> onBoth)
    {
        return result.IsSuccess
            ? Result.Success(onSuccess(result.Value).Result)
            : Result.Success(onBoth(result.Value, result.Error).Result);
    }

    public static Result<T> MatchAsync<T>(this Result<T> result, Func<T, Task<T>> onSuccess,
        Func<Error, Task<T>> onFailure, Func<T, Error, Task<Result<T>>> onBoth)
    {
        return result.IsSuccess
            ? Result.Success(onSuccess(result.Value).Result)
            : onBoth(result.Value, result.Error).Result;
    }
}