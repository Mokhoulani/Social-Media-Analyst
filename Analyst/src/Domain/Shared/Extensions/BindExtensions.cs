namespace Domain.Shared.Extensions;

public static class BindExtensions
{
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess ? func(result.Value) : Result.Failure<TOut>(result.Error);
    }

    public static Result<TOut> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Result<TOut>> func)
    {
        var result = resultTask.Result;
        return result.IsSuccess ? func(result.Value) : Result.Failure<TOut>(result.Error);
    }

    public static Result<TOut> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = resultTask.Result;
        return result.IsSuccess ? func(result.Value).Result : Result.Failure<TOut>(result.Error);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Result<T> result, Func<T, Task<Result<TOut>>> func)
    {
        if (result.IsFailure) return Result.Failure<TOut>(result.Error);
        return await func(result.Value);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Task<Result<T>> resultTask,
        Func<T, Task<Result<TOut>>> func)
    {
        var result = await resultTask;
        if (result.IsFailure) return Result.Failure<TOut>(result.Error);
        return await func(result.Value);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Task<Result<T>> resultTask,
        Func<T, Result<TOut>> func)
    {
        var result = await resultTask;
        if (result.IsFailure) return Result.Failure<TOut>(result.Error);
        return func(result.Value);
    }

    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Action<T> action)
    {
        var result = await resultTask;
        if (result.IsSuccess) action(result.Value);
        return result;
    }
}