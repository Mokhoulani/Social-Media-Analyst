using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.User.Commands;
using Application.Services;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;



namespace Application.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(environment);
        
        // Register MediatR
        services.AddMediatR(typeof(AssemblyReference).Assembly);
      
        ConfigureValidation(services);
        ConfigureProblemDetails(services, environment);
        ConfigureMapping(services);
        
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<EmailService>();
        return services;
    }


    /// <summary>
    /// Mapster(Mapper) global configuration settings
    /// To learn more about Mapster,
    /// see https://github.com/MapsterMapper/Mapster
    /// </summary>
    /// <returns></returns>
    private static TypeAdapterConfig GetConfiguredMappingConfig()
    {
        var config = new TypeAdapterConfig();

        // Scan the current assembly for mapping configurations
        config.Scan(typeof(DependencyInjection).Assembly);

        return config;
    }
    
    private static void ConfigureValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(SignUpValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
    
    private static void ConfigureMapping(IServiceCollection services)
    {
        var mappingConfig = GetConfiguredMappingConfig();
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        var codeGenerationConfig = new CodeGenerationConfig();
        var modelCodeGenRegister = new ModelCodeGenRegister();
        modelCodeGenRegister.Register(codeGenerationConfig);

        services.AddSingleton(codeGenerationConfig);
    }
    private static void ConfigureProblemDetails(IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddProblemDetails(opts =>
        {
            // Disable exception details in production
            opts.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();
            
            // opts.IncludeExceptionDetails = (ctx, ex) => false;

            // Configure handling for ValidationException
            opts.Map<ValidationException>((ctx, ex) =>
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
                    );

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "One or more validation errors occurred."
                };

                // Optional: Add exception details for development
                if (environment.IsDevelopment())
                {
                    problemDetails.Extensions["exceptionDetails"] = new[]
                    {
                        new
                        {
                            message = ex.Message,
                            type = ex.GetType().FullName,
                            raw = ex.ToString()
                        }
                    };
                }

                return problemDetails;
            });

            // Map other exceptions to ProblemDetails
            opts.Map<System.Exception>((ctx, ex) =>
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please try again later."
                };

                // Optional: Add exception details for development
                if (environment.IsDevelopment())
                {
                    problemDetails.Extensions["exceptionDetails"] = new[]
                    {
                        new
                        {
                            message = ex.Message,
                            type = ex.GetType().FullName,
                            raw = ex.ToString()
                        }
                    };
                }

                return problemDetails;
            });
        });
    }
}
