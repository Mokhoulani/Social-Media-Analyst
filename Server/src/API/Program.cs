using Api.OptionsSetup;
using Application.Common.Extensions;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz;
using Serilog;

public partial class Program
{
    public static void Main(string[] args)
    {
        ConfigureLogger();

        try
        {
            StartApplication(args);
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Error during startup");
        }
        finally
        {
            Log.Information("Shutting down");
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    private static void StartApplication(string[] args)
    {
        Log.Information("Starting up");

        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        Configure(builder.Build()).Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
        builder.Services.AddControllers();

        builder.Services.AddInfrastructureLayer(builder.Configuration);
        builder.Services.AddApplicationLayer(builder.Environment);

        builder.Services.AddQuartzHostedService();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureOptions<JwtOptionsSetup>();
        builder.Services.ConfigureOptions<RefreshTokenOptionsSetup>();
        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        builder.Services.AddOpenApi();
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        builder.Services.AddHealthChecks();
    }

    private static WebApplication Configure(WebApplication app)
    {
        
        app.UseDefaultFiles();
        app.MapStaticAssets();


        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapReverseProxy();
        app.UseSerilogRequestLogging();
        app.UseProblemDetails();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHealthChecks("/health");

        app.MapControllers();
        app.MapFallbackToFile("/index.html");
        app.Run();
        return app;
    }
}



public partial class Program { }