using Application.Abstractions.Messaging;
using Domain.Shared;
using MediatR;

namespace Application.CQRS.Authentication.Commands;

public record RequestPasswordResetCommand(string Email) : ICommand<Result<bool>>;


