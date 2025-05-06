namespace Domain.Shared.Extensions
{
    public static class EnsureExtensions
    {
        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
        {
            if (result.IsFailure) return result;
            return predicate(result.Value) ? result : Result.Failure<T>(error);
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Func<Error> errorFunc)
        {
            if (result.IsFailure) return result;
            return predicate(result.Value) ? result : Result.Failure<T>(errorFunc());
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Error> errorFunc)
        {
            if (result.IsFailure) return result;
            return predicate(result.Value) ? result : Result.Failure<T>(errorFunc(result.Value));
        }
    }
}