using Microsoft.Extensions.DependencyInjection;

namespace Netsoftware.Xanthos.Api.StartupExtensions;

public static class AddCorsPoliciesExtension
{
    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevCorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            options.AddPolicy("ProdCorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });
        return services;
    }
}