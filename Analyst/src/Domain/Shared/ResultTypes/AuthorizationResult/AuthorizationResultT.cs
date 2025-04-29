using Domain.Errors;

namespace Domain.Shared.ResultTypes.AuthorizationResult;

public class AuthorizationResult<T> : Result<T>, IAuthorizationResult
{
    internal AuthorizationResult(Error[] errors, Error baseError)
        : base(default, false, baseError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static AuthorizationResult<T> Authorized(string code, string message) =>
        new([new Error(code, message)],AuthorizationErrors.Forbidden(message));
}