using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;


namespace Application.Common.Extensions;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            var result = HandleException(ex);
            await result.ExecuteResultAsync(new ActionContext
            {
                HttpContext = context,
                RouteData = context.GetRouteData() ?? new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            });
        }
    }

    private IActionResult HandleException(System.Exception ex)
    {
        return ex switch
        {
            ValidationException validationEx => new BadRequestObjectResult(
                new ValidationProblemDetails(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
                        ))
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest
                }),
            
            _ => new ObjectResult(new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}


