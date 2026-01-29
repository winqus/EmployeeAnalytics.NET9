using EmpAnalytics.API.Controllers;
using EmpAnalytics.API.Extensions;
using EmpAnalytics.API.Middleware;
using EmpAnalytics.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "EmployeeAnalyticsAPI";
    config.Title = "EmployeeAnalyticsAPI v1";
    config.Version = "v1";
});

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.DocumentTitle = "EmployeeAnalyticsAPI";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});

app.ApplyMigrations();
await app.SeedDatabaseAsync(useLargeSeed: true);


app.MapHealthChecks("/health");

app.MapUsersEndpoints();

app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

public abstract partial class Program;