using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.CQRS.Authentication.Commands;

namespace Application.CQRS.Authentication.Handlers;

// public class ResetPasswordHandler(IPasswordResetService passwordResetService) : ICommandHandler<ResetPasswordCommand>
// {
//     public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
//     {
//         await passwordResetService.ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
//     }
// }
