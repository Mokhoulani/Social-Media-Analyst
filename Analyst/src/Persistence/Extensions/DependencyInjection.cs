using Domain.Interfaces;
using Persistence.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Persistence.Repositories;



namespace Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(
        this IServiceCollection services)
    {

        services.AddScoped<Dictionary<Type, object>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}