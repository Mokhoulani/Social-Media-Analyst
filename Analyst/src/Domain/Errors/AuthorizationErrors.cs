using Domain.Shared;

namespace Domain.Errors;

public static class AuthorizationErrors
{
    public static Error Forbidden(string permission) =>
        new("Authorization.Forbidden", $"You do not have the required permission: '{permission}'.");
    
    public static Error Forbidden() => new("Authorization.Forbidden", "You do not have access to this resource.");
}
