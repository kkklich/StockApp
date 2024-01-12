using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Utils.Database.StartupExtensions;

public static class DataSeedExtensions
{
    public static IApplicationBuilder ApplicationInstanceModuleSeedData(this IApplicationBuilder builder)
    {
        var provider = builder.ApplicationServices;
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using var scope = scopeFactory.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        initializer.Initialize().Wait();
        return builder;
    }
}