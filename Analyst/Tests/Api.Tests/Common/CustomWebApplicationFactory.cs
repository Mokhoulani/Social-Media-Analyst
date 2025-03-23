using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Persistence;


namespace Api.Tests.Common;

public class WebapiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            string connectionString = GetConnectionString() ?? "DataSource=file::memory:?cache=shared";
            services.AddSqlite<ApplicationDbContext>(connectionString);

            ApplicationDbContext dbContextApp = CreateDbContext(services);

            dbContextApp.Database.EnsureDeleted();
            dbContextApp.Database.EnsureCreated();

            SeedTestData(dbContextApp);
        });
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables() // Add environment variables
            .AddUserSecrets<WebapiWebApplicationFactory>()
            .Build();
        return configuration.GetConnectionString("testDb");
    }

    private static ApplicationDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context;
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        var emailResult = Email.Create("johndoe@example.com");
        var firstNameResult = FirstName.Create("John");
        var lastNameResult = LastName.Create("Doe");
        var passwordResult = Password.Create("SecurePassword123!");

        var testUser = User.Create(
            Guid.Parse("70ebadd4-e923-4584-b82f-52175d8c80db"),
            emailResult.Value,
            firstNameResult.Value,
            lastNameResult.Value,
            passwordResult.Value
        );

        context.Set<User>().Add(testUser);

        context.SaveChanges();
    }
}