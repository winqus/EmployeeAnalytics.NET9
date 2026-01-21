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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://127.0.0.1:5173"
        );
    });
});

builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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
}

app.MapUsersEndpoints();

app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();