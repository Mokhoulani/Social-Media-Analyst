using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Common.Extensions;
public static class ProblemDetailsExtensions
{
    public static void AddProblemDetailsConfiguration(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.AddProblemDetails(opts =>
        {
            opts.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

            var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ProblemDetails");

            opts.Map<Exception>((ctx, ex) =>
            {
                logger.LogError(ex, "An unexpected exception occurred.");
                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please try again later."
                };
                return problemDetails;
            });
        });
    }
}

