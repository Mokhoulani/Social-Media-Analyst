namespace Domain.Shared.ResultTypes.ValidationResult;

public interface IValidationResult
{
    Error[] Errors { get; }
}
