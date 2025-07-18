using Application.Common.Mod;
using Application.Common.Mod.Abstraction;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;

namespace Application.Common.Interfaces;

public interface IAuthService
{
    Task<Result<TokenResponseViewModel>> LoginAsync(LoginCommand command, CancellationToken cancellationToken);
    Task<Result<TokenResponseViewModel>> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    Task<Result<TokenResponseViewModel>> GenerateTokenResponse(User user, CancellationToken cancellationToken);
}