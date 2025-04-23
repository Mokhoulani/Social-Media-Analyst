using Domain.Shared;
using Domain.Shared.ResultTypes.AuthenticationResult;
using Domain.Shared.ResultTypes.ValidationResult;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions;

[ApiController]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected IActionResult HandleFailure(Result result) =>
     result switch
     {
         { IsSuccess: true } => throw new InvalidOperationException(),

         IValidationResult validationResult =>
             BadRequest(CreateProblemDetails(
                 "Validation Error",
                 StatusCodes.Status400BadRequest,
                 result.Error,
                 validationResult.Errors)),

         IAuthenticationResult authenticationResult =>
             StatusCode(StatusCodes.Status401Unauthorized,
                 CreateProblemDetails(
                     "Unauthenticated",
                     StatusCodes.Status401Unauthorized,
                     result.Error,
                     authenticationResult.Errors)),

         _ => BadRequest(CreateProblemDetails(
             "Bad Request",
             StatusCodes.Status400BadRequest,
             result.Error))
     };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
