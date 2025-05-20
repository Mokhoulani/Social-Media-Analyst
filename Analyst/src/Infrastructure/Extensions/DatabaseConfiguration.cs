using Infrastructure.BackgroundJobs;
using Persistence.Interceptors;
using Infrastructure.OptionsSetup;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Persistence;
using Quartz;
using Scrutor;

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
                  Infrastructure.AssemblyReference.Assembly,
                  Persistence.AssemblyReference.Assembly)
              .AddClasses(false)
              .UsingRegistrationStrategy(RegistrationStrategy.Skip)
              .AsMatchingInterface()
              .WithScopedLifetime());



        services.ConfigureOptions<SqlOptionsSetup>();
        var sqlOptions = services.BuildServiceProvider().GetRequiredService<IOptions<SqlOptions>>().Value;

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                var convertInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                var auditInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

                optionsBuilder.UseSqlite(
                        sqlOptions.ConnectionString,
                        b => b.MigrationsAssembly("Persistence"))
                    .AddInterceptors(convertInterceptor, auditInterceptor)
                    .EnableSensitiveDataLogging();
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
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));

        });

        services.AddHealthChecks().AddSqlite(sqlOptions.ConnectionString);
    }
}