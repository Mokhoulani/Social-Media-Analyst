using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.CQRS.Authentication.Commands;



namespace Application.CQRS.Authentication.Handlers;

public class RequestPasswordResetHandler(IPasswordResetService passwordResetService)
    : ICommandHandler<RequestPasswordResetCommand , bool>
{
    public async Task<bool> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
       return await passwordResetService
           .RequestPasswordResetAsync(request.Email, cancellationToken);
    }
}
