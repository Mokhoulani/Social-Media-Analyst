using Application.Abstractions.Messaging;
using MediatR;

namespace Application.CQRS.Authentication.Commands;

public record RequestPasswordResetCommand(string Email) : ICommand<bool>;


