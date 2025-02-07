using Application.Common.Mod;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;

namespace Application.Common.Interfaces;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
}