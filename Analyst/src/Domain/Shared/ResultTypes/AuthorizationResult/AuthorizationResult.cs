using Domain.Errors;

namespace Domain.Shared.ResultTypes.AuthorizationResult;

public class AuthorizationResult : Result, IAuthorizationResult
{
    internal AuthorizationResult(Error[] errors, Error baseError)
        : base(false, baseError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static AuthorizationResult Unauthorized(string code, string message)
    {
        return new AuthorizationResult([new Error(code, message)], AuthorizationErrors.Forbidden());
    }

    public static AuthorizationResult Unauthorized(Error[] errors)
    {
        return new AuthorizationResult(errors, AuthorizationErrors.Forbidden());
    }

    public static AuthorizationResult Unauthorized()
    {
        return new AuthorizationResult([new Error("User.Forbidden", "You do not have the required permission.")],
            AuthorizationErrors.Forbidden());
    }

    public static AuthorizationResult Unauthorized(Error error)
    {
        return new AuthorizationResult([error], AuthorizationErrors.Forbidden());
    }

    public static AuthorizationResult Unauthorized(Error[] errors, string code, string message)
    {
        return new AuthorizationResult(errors, new Error(code, message));
    }
}