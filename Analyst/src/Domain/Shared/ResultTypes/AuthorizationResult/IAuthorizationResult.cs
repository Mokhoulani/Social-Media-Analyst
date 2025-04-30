namespace Domain.Shared.ResultTypes.AuthorizationResult;

public interface IAuthorizationResult
{
    Error[] Errors { get; }
}