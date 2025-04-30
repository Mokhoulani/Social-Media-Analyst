using Domain.Errors;

namespace Domain.Shared.ResultTypes.AuthenticationResult;

public sealed class AuthenticationResult : Result, IAuthenticationResult
{
    internal AuthenticationResult(Error[] errors, Error baseError)
        : base(false, baseError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static AuthenticationResult Unauthenticated(string code, string message) =>
        new([new Error(code, message)], AuthenticationErrors.Unauthenticated);
    public static AuthenticationResult Unauthenticated(Error[] errors) =>
        new(errors, AuthenticationErrors.Unauthenticated);
    public static AuthenticationResult Unauthenticated() =>
        new([new Error("User.Unauthenticated", "User is not authenticated.")], AuthenticationErrors.Unauthenticated);
    public static AuthenticationResult Unauthenticated(Error error) =>
        new([error], AuthenticationErrors.Unauthenticated);
    public static AuthenticationResult Unauthenticated(Error[] errors, string code, string message) =>
        new(errors, new Error(code, message));
}