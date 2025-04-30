using Domain.Shared;

namespace Domain.Errors;

public static class ValidationErrors
{
    public static readonly Error Default = new(
        "Validation",
        "A validation error occurred.");
}