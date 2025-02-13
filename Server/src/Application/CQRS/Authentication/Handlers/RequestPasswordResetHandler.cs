using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.CQRS.Authentication.Commands;
using MediatR;


// namespace Application.CQRS.Authentication.Handlers;
//
// public class RequestPasswordResetHandler(IPasswordResetService passwordResetService)
//     : ICommandHandler<RequestPasswordResetCommand>
// {
//     public async Task Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
//     {
//         await passwordResetService.RequestPasswordResetAsync(request.Email, cancellationToken);
//     }
// }
