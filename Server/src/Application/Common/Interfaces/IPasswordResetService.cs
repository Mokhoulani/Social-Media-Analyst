
using Domain.Shared;

namespace Application.Common.Interfaces;

public interface IPasswordResetService
{
    Task<Result<bool>> RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task<Result<bool>> ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken);
}