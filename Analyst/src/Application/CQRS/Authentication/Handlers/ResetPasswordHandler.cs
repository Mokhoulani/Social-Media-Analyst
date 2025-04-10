using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Authentication.Commands;
using Domain.Shared;

namespace Application.CQRS.Authentication.Handlers;

public class ResetPasswordHandler(IPasswordResetService passwordResetService) :
    ICommandHandler<ResetPasswordCommand, PasswordResetViewModel>
{
    public async Task<Result<PasswordResetViewModel>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
      return  await passwordResetService
          .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
    }
}
