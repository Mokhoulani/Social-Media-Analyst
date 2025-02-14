
namespace Application.Common.Interfaces;

public interface IPasswordResetService
{
    Task<bool> RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken);
}