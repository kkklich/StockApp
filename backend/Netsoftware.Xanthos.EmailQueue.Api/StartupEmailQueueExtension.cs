using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.EmailQueue.Database.Repositories;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Mapping;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Repositories;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Services;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Services.Interfaces;

namespace Netsoftware.Xanthos.EmailQueue.Api;

public static class StartupEmailQueueExtension
{
    public static IServiceCollection AddEmailQueueModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMvcCore().AddApplicationPart(typeof(StartupEmailQueueExtension).Assembly);
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IEmailQueueService, EmailQueueService>();
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}