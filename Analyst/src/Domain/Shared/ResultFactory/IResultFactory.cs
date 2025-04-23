using Domain.Shared.ResultTypes.AuthenticationResult;
using Domain.Shared.ResultTypes.ValidationResult;

namespace Domain.Shared.ResultFactory
{
    public interface IResultFactory
    {
        ValidationResult Validation(Error[] errors);
        ValidationResult<T> Validation<T>(Error[] errors);

        AuthenticationResult Authentication(Error[] errors);
        AuthenticationResult<T> Authentication<T>(Error[] errors);

        Result<T> Success<T>(T value);
        Result Success();
    }
}