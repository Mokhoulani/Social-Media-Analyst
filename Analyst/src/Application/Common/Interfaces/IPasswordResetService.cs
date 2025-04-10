using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.Common.Interfaces;

public interface IPasswordResetService
{
    Task<Result<PasswordResetViewModel>> RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task<Result<PasswordResetViewModel>> ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken);
}