namespace Domain.Shared.Extensions;

public static class TapExtensions
{
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess) action(result.Value);
        return result;
    }

    public static Result<T> Tap<T>(this Result<T> result, Func<T, Task> action)
    {
        if (result.IsSuccess) action(result.Value).Wait();
        return result;
    }

    public static Result<T> Tap<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
    {
        var result = resultTask.Result;
        if (result.IsSuccess) action(result.Value).Wait();
        return result;
    }

    public static Result<T> Tap<T>(this Task<Result<T>> resultTask, Action<T> action)
    {
        var result = resultTask.Result;
        if (result.IsSuccess) action(result.Value);
        return result;
    }

    public static async Task<Result<T>> TapAsync<T>(this Result<T> resultTask, Func<T, Task> action)
    {
        if (resultTask.IsSuccess) await action(resultTask.Value);
        return resultTask;
    }

    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Action<T> action)
    {
        var result = await resultTask;
        if (result.IsSuccess) action(result.Value);
        return result;
    }

    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess) await action(result.Value);
        return result;
    }
}