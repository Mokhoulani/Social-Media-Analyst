using Domain.Errors;

namespace Domain.Shared.ResultTypes.AuthenticationResult;

public sealed class AuthenticationResult<T> : Result<T>,
    IAuthenticationResult
{
    internal AuthenticationResult(Error[] errors,
        Error baseError)
        : base(default,
            false,
            baseError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static AuthenticationResult<T> Unauthenticated(string code,
        string message) =>
        new([
                new Error(code,
                    message)
            ],
            AuthenticationErrors.Unauthenticated);

    public static AuthenticationResult<T> Unauthenticated(Error[] errors) =>
        new(errors,
            AuthenticationErrors.Unauthenticated);
}