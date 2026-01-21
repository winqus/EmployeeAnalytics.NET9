using EmpAnalytics.Application.Data;
using EmpAnalytics.Application.Users.Get;
using EmpAnalytics.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmpAnalytics.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IGetTop10RecentlyActiveEmployees, GetTop10RecentlyActiveEmployees>();

        return services;
    }
}