using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<CompanyDbContext>(
            builder => builder.UseNpgsql(connectionString));

        services.AddScoped<ICompanyDbContext>(
            provider => provider.GetService<CompanyDbContext>()!);

        return services;
    }
}