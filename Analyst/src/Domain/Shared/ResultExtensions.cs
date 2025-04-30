namespace Domain.Shared;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if (result.IsFailure)
            return result;
        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result,
        Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Error);
    }

    public static Result<T> Tap<T>(this Result<T> result,
        Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);

        return result;
    }

    public static Result<T> MapError<T>(this Result<T> result,
        Func<Error, Error> errorMappingFunc)
    {
        return result.IsFailure
            ? Result.Failure<T>(errorMappingFunc(result.Error))
            : result;
    }

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result,
        Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess
            ? func(result.Value)
            : Result.Failure<TOut>(result.Error);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Result<T> result,
        Func<T, Task<Result<TOut>>> func)
    {
        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);

        return await func(result.Value);
    }

    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask,
        Action<T> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            action(result.Value);
        return result;
    }

    public static async Task<Result<TResult>> MapAsync<T, TResult>(this Task<Result<T>> resultTask,
        Func<T, Task<TResult>> map)
    {
        var result = await resultTask;

        if (result.IsFailure)
            return Result.Failure<TResult>(result.Error);

        var mappedResult = await map(result.Value);
        return Result.Success(mappedResult);
    }
}