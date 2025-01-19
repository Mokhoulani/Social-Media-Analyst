using Api.OptionsSetup;
using Application.Common.Extensions;
using Infrastructure.Persistence;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.BackgroundJobs;
using Infrastructure.Interceptors;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz;
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly)
            .AddClasses(false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

string connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddApplicationLayer();

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, optionsBuilder) =>
    {
        var inteceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

        optionsBuilder.UseSqlite(connectionString,b=>b.MigrationsAssembly("API"))
            .AddInterceptors(inteceptor);
    });

builder.Services.AddQuartz(configure =>
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

builder.Services.AddQuartzHostedService();


builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


