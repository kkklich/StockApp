using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.ApplicationInstance.Database.Repositories;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Mapping;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers.Interfaces;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Repositories;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services.Interfaces;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Utils.Database;

namespace Netsoftware.Xanthos.ApplicationInstance.Api;

public static class StartupApplicationInstanceExtension
{
    public static IServiceCollection AddApplicationInstanceModule(this IServiceCollection services)
    {
        services.AddMvcCore().AddApplicationPart(typeof(StartupApplicationInstanceExtension).Assembly);
        services.AddTransient<IDbInitializer, DbInitializer>()
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IApplicationInstancesService, ApplicationInstancesService>()
            .AddScoped<IUsersDataProvider, UsersDataProvider>()
            .AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}