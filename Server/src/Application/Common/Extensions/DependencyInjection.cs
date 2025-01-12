using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Handlers;
using Application.Interfaces;
using Application.Services;
using Domain.Events;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Domain.Interfaces;




namespace Application.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(typeof(DependencyInjection).Assembly);


        services.AddScoped<IUserService, UserService>();

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(SignUpValidator).Assembly);

        // Register ValidationBehavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        // Register Mapper
        services.AddSingleton(GetConfiguredMappingConfig());

        var codeGenerationConfig = new CodeGenerationConfig();
        var modelCodeGenRegister = new ModelCodeGenRegister();
        modelCodeGenRegister.Register(codeGenerationConfig);

        services.AddSingleton(codeGenerationConfig);
        services.AddScoped<IMapper, ServiceMapper>();

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
        var config = TypeAdapterConfig.GlobalSettings;

        var registers = config.Scan(Assembly.GetExecutingAssembly());

        config.Apply(registers);

        return config;
    }
}
