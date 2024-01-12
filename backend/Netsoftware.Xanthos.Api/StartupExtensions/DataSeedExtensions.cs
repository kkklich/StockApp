using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.Infrastructure.Utils.Database;

namespace Netsoftware.Xanthos.Api.StartupExtensions;

public static class DataSeedExtensions
{
    public static void SeedData(this IApplicationBuilder builder)
    {
        var provider = builder.ApplicationServices;
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using (var scope = scopeFactory.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            initializer.Initialize().Wait();
        }
    }
}