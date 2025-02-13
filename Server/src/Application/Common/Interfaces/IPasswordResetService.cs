namespace Application.Common.Interfaces;

public interface IPasswordResetService
{
    Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken);
}