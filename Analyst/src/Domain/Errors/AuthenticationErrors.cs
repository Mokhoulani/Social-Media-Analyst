using Domain.Shared;

namespace Domain.Errors;

public static class AuthenticationErrors
{
        public static readonly Error Unauthenticated = new(
            "User.Unauthenticated",
            "User is not authenticated.");    
}