using Infrastructure.BackgroundJobs;
using Persistence.Interceptors;
using Infrastructure.OptionsSetup;
using Persistence;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Persistence;
using Quartz;

namespace Infrastructure.Extensions;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Scan(
                selector => selector
                    .FromAssemblies(
                        Infrastructure.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        
        services.ConfigureOptions<SqlOptionsSetup>();
        var sqlOptions = services.BuildServiceProvider().GetRequiredService<IOptions<SqlOptions>>().Value;
        
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                if (interceptor != null)
                    optionsBuilder.UseSqlite(
                            sqlOptions.ConnectionString,
                            b => b.MigrationsAssembly("Infrastructure"))
                        .AddInterceptors(interceptor);
            });
        
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });
       
        services.AddHealthChecks().AddSqlite(sqlOptions.ConnectionString);
    }
}