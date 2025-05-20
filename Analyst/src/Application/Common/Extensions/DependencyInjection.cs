using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.User.Validators;
using Application.Services;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;



namespace Application.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        services.AddMediatR(typeof(AssemblyReference).Assembly);

        ConfigureValidation(services);
        ConfigureCacheBehavior(services);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>));
        services.AddProblemDetailsConfiguration(environment);
        
        services.AddSingleton(GetConfiguredMappingConfig());
        
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IUserDeviceService, UserDeviceService>();

        services.AddTransient<EmailService>();
        return services;
    }
    
    private static TypeAdapterConfig GetConfiguredMappingConfig()
    {
        var config = TypeAdapterConfig.GlobalSettings;
    
        var registers = config.Scan(Assembly.GetExecutingAssembly());
    
        config.Apply(registers);
    
        return config;
    }

    private static void ConfigureValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(SignUpValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void ConfigureCacheBehavior(IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
    }
}
