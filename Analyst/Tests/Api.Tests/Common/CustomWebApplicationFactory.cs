using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            var connectionString = GetConnectionString() ?? "DataSource=file::memory:?cache=shared";
            services.AddSqlite<ApplicationDbContext>(connectionString);

            var dbContextApp = CreateDbContext(services);

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
    private static void SeedTestData(ApplicationDbContext context)
    {
        // 1. Seed Permissions
        var permissions = new List<Permission>
    {
        new(1, "ReadUser"),
        new(2, "UpdateUser"),
        new(3, "DeleteUser"),
        new(4, "CreateUser"),
        new(5, "ReadRole"),
        new(6, "UpdateRole"),
        new(7, "DeleteRole"),
        new(8, "CreateRole"),
        new(9, "ReadPermission"),
        new(10, "UpdatePermission"),
        new(11, "DeletePermission"),
        new(12, "CreatePermission")
    };

        context.Set<Permission>().AddRange(permissions);

        // 2. Seed Roles
        var registeredRole = new Role(1, "Registered");
        context.Set<Role>().Add(registeredRole);

        // 3. Seed RolePermissions (Registered Role has User permissions)
        var rolePermissions = new List<RolePermission>
    {
        new(1, 1),
        new(1, 2),
        new(1, 3),
        new(1, 4)
    };

        context.Set<RolePermission>().AddRange(rolePermissions);

        // 4. Seed Test User
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

        // 5. Assign Registered role to user
        testUser.Roles.Add(registeredRole);

        context.Set<User>().Add(testUser);

        // 6. Commit all data
        context.SaveChanges();
    }
}