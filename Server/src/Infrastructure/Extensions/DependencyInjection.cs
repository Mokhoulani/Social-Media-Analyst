using Application.Common.Interfaces;
using Domain.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }
}
