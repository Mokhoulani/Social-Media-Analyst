using Domain.Errors;

namespace Domain.Shared.ResultTypes.ValidationResult;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    internal ValidationResult(Error[] errors, Error baseError)
        : base(default, false, baseError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) =>
     new(errors, ValidationErrors.Default);
    public static ValidationResult<TValue> WithErrors(Error error) =>
    new([error], ValidationErrors.Default);
    public static ValidationResult<TValue> WithErrors(Error[] errors, string code, string message) =>
        new(errors, new Error(code, message));
    public static ValidationResult<TValue> WithErrors(string code, string message) =>
        new([new Error(code, message)], ValidationErrors.Default);
    public static ValidationResult<TValue> WithErrors() =>
        new([new Error("Validation.Default", "Validation failed.")], ValidationErrors.Default);
}
