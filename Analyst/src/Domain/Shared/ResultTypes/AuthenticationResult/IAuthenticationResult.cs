namespace Domain.Shared.ResultTypes.AuthenticationResult;

public interface IAuthenticationResult
{
    Error[] Errors { get; }
}