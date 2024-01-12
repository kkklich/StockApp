using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Netsoftware.Xanthos.Common.DatabaseConfiguration.StartupExtensions;

public static class CustomDatabaseConfigurationExtension
{
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, IConfiguration configuration,
        [NotNull] string connectionStringName) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(connectionStringName),
                x => x.MigrationsHistoryTable(connectionStringName + "_MigrationHistory")));
        return services;
    }
}