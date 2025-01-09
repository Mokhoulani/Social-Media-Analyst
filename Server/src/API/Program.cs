using Application.Common.Extensions;
using Infrastructure.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Create a single SQLite connection
var connection = new SqliteConnection("Data Source=:memory:");
connection.Open(); // Keep the connection open for the application's lifetime

// Add DbContext with the shared connection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connection);
});

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application and infrastructure layer services
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureServices();

// Add OpenAPI/Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations and seed data
await ApplyMigrationsAndSeedAsync(app);

// Enable authentication and authorization middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// Helper to apply migrations and seed data
static async Task ApplyMigrationsAndSeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Apply migrations
    await context.Database.EnsureCreatedAsync();
 
}