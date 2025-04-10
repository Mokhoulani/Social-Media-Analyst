using Domain.Shared;

namespace Domain.Errors;

public static class AuthenticationErrors
{
    public static class User
    {
        public static readonly Error Unauthorized = new(
            "User.Unauthorized",
            "User is not authenticated.");
    }
}