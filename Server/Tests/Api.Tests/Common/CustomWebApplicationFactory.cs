using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.Tests.Common;

public class WebapiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContextOptions configuration
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            // Add SQLite in-memory database for testing
            string? connectionString = GetConnectionString();
            services.AddSqlite<ApplicationDbContext>(connectionString);

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to resolve the DbContext
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is deleted and recreated
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Seed test data
            SeedTestData(dbContext);
        });
    }
    
    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<WebapiWebApplicationFactory>() // ✅ Loads from User Secrets
            .Build();
        return configuration.GetConnectionString("testDb");
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        // Create test data
        var email = Email.Create("johndoe@example.com");
        var firstName = FirstName.Create("John");
        var lastName = LastName.Create("Doe");
        var password = Password.Create("SecurePassword123!");

        var testUser = User.Create(
            Guid.Parse("70ebadd4-e923-4584-b82f-52175d8c80db"),
            email,
            firstName,
            lastName,
            password
        );

        // Add test data to the database
        context.Set<User>().Add(testUser);
         context.SaveChanges();
    }
}