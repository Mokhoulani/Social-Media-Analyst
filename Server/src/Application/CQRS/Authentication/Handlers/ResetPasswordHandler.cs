using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.CQRS.Authentication.Commands;
using Domain.Shared;

namespace Application.CQRS.Authentication.Handlers;

public class ResetPasswordHandler(IPasswordResetService passwordResetService) :
    ICommandHandler<ResetPasswordCommand, bool>
{
    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
      return  await passwordResetService
          .ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);
    }
}
