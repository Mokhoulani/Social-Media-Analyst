namespace Domain.Shared.Extensions;

public static class MapExtensions
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess ? Result.Success(mappingFunc(result.Value)) : Result.Failure<TOut>(result.Error);
    }

    public static Result<TOut> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mappingFunc)
    {
        return result.IsSuccess ? Result.Success(mappingFunc(result.Value).Result) : Result.Failure<TOut>(result.Error);
    }

    public static Result<T> MapError<T>(this Result<T> result, Func<Error, Error> errorMappingFunc)
    {
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc(result.Error)) : result;
    }

    public static async Task<Result<TResult>> MapAsync<T, TResult>(this Task<Result<T>> resultTask,
        Func<T, Task<TResult>> map)
    {
        var result = await resultTask;
        if (result.IsFailure) return Result.Failure<TResult>(result.Error);
        var mappedResult = await map(result.Value);
        return Result.Success(mappedResult);
    }

    public static async Task<Result<TResult>> MapAsync<T, TResult>(this Task<Result<T>> resultTask,
        Func<T, TResult> map)
    {
        var result = await resultTask;
        if (result.IsFailure) return Result.Failure<TResult>(result.Error);
        var mappedResult = map(result.Value);
        return Result.Success(mappedResult);
    }

    public static async Task<Result<T>> MapErrorAsync<T>(this Task<Result<T>> resultTask,
        Func<Error, Error> errorMappingFunc)
    {
        var result = await resultTask;
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc(result.Error)) : result;
    }

    public static async Task<Result<T>> MapErrorAsync<T>(this Task<Result<T>> resultTask, Func<Error> errorMappingFunc)
    {
        var result = await resultTask;
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc()) : result;
    }

    public static async Task<Result<T>> MapErrorAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> errorMappingFunc)
    {
        var result = await resultTask;
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc(result.Value)) : result;
    }

    public static async Task<Result<T>> MapErrorAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> errorMappingFunc, Func<Error> errorFunc)
    {
        var result = await resultTask;
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc(result.Value)) : result;
    }

    public static Result<T> MapError<T>(this Result<T> result, Func<Error> errorMappingFunc)
    {
        return result.IsFailure ? Result.Failure<T>(errorMappingFunc()) : result;
    }
}