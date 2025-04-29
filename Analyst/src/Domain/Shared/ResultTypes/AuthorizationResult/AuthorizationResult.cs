using Domain.Errors;

namespace Domain.Shared.ResultTypes.AuthorizationResult;

public class AuthorizationResult : Result, IAuthorizationResult
{
    internal AuthorizationResult(Error[] errors, Error baseError)
        : base(false, baseError) =>
        Errors = errors;

    public Error[] Errors { get; }
    
    public static AuthorizationResult Authorized(string code, string message) =>
        new([new Error(code, message)], AuthorizationErrors.Forbidden());
    public static AuthorizationResult Authorized(Error[] errors) =>
        new(errors, AuthorizationErrors.Forbidden());
    public static AuthorizationResult Authorized() =>
        new([new Error("User.Forbidden", "You do not have the required permission.")],AuthorizationErrors.Forbidden());
    public static AuthorizationResult Authorized(Error error) =>
        new([error], AuthorizationErrors.Forbidden());
    public static AuthorizationResult Authorized(Error[] errors, string code, string message) =>
        new(errors, new Error(code, message));
}

