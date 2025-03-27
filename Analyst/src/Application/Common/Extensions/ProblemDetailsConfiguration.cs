using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Common.Extensions;

public static class ProblemDetailsConfiguration
{
    public static void AddProblemDetailsConfiguration(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddProblemDetails(opts =>
      {
          opts.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

          var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
          var logger = loggerFactory.CreateLogger("ProblemDetails");

          opts.Map<ValidationException>((ctx, ex) =>
          {
              logger.LogWarning("ValidationException occurred: {Message}", ex.Message);
              return CreateProblemDetails(
                  "Validation Error",
                  StatusCodes.Status400BadRequest,
                  "One or more validation errors occurred.",
                  ex.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(
                          g => g.Key,
                          g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
                      ),
                  ex,
                  environment,
                  logger);
          });

          opts.Map<System.Exception>((ctx, ex) =>
          {
              logger.LogError(ex, "An unexpected exception occurred.");
              return CreateProblemDetails(
                  "An unexpected error occurred",
                  StatusCodes.Status500InternalServerError,
                  "Please try again later.",
                  null,
                  ex,
                  environment,
                  logger);
          });
      });
    }

    private static ProblemDetails CreateProblemDetails(
      string title,
      int status,
      string detail,
      object? errors = null,
      Exception? exception = null,
      IWebHostEnvironment? environment = null,
      ILogger? logger = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = status,
            Detail = detail,
            Extensions = { { "errors", errors } }
        };

        if (environment?.IsDevelopment() == true && exception is not null)
        {
            problemDetails.Extensions["exceptionDetails"] = new
            {
                message = exception.Message,
                type = exception.GetType().FullName,
                raw = exception.ToString()
            };
        }

        return problemDetails;
    }
}