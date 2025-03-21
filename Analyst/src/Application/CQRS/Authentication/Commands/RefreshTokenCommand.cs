using Application.Abstractions.Messaging;
using Application.Common.Mod;
using Domain.Shared;


namespace Application.CQRS.Authentication.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<Result<TokenResponse>>;