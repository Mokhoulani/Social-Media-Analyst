using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Authentication.Commands;
using Domain.Shared;


namespace Application.CQRS.Authentication.Handlers;

public class RequestPasswordResetHandler(IPasswordResetService passwordResetService)
    : ICommandHandler<RequestPasswordResetCommand , PasswordResetViewModel>
{
    public async Task<Result<PasswordResetViewModel>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
      return await passwordResetService
           .RequestPasswordResetAsync(request.Email, cancellationToken);
    }
}
