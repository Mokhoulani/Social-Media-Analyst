using Application.Abstractions.Messaging;
using Application.Common.Mod;
using Application.Common.Mod.ViewModels;


namespace Application.CQRS.Authentication.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResponse>;