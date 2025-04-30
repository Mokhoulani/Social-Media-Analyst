using Api.OptionsSetup;
using Application.Common.Extensions;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Extensions;
using Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder
    .Services
    .AddControllers()
    .AddApplicationPart(AssemblyReference.Assembly);

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
builder.Services.AddHealthChecks();

var app = builder.Build();


app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


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



public partial class Program { }
