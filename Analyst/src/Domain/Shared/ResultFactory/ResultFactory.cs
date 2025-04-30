using Domain.Errors;
using Domain.Shared.ResultTypes.AuthenticationResult;
using Domain.Shared.ResultTypes.ValidationResult;

namespace Domain.Shared.ResultFactory
{
    public static class ResultFactory
    {
        public static ValidationResult Validation(Error[] errors) =>
            new(errors, ValidationErrors.Default);

        public static ValidationResult<T> Validation<T>(Error[] errors) =>
            new(errors, ValidationErrors.Default);

        public static AuthenticationResult Authentication(Error[] errors) =>
            new(errors, AuthenticationErrors.Unauthenticated);

        public static AuthenticationResult<T> Authentication<T>(Error[] errors) =>
            new(errors, AuthenticationErrors.Unauthenticated);

        public static Result<T> Success<T>(T value) => new(value, true, null!);

        public static Result Success() => new(true, null!);
    }
}