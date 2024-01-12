using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Netsoftware.Xanthos.EmailQueue.HostService;

public static class StartupHostServiceExtension
{
    public static IServiceCollection AddEmailQueueHostServiceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMvcCore().AddApplicationPart(typeof(StartupHostServiceExtension).Assembly);
        services.AddHostedService<EmailQueueHostService>();
        return services;
    }
}