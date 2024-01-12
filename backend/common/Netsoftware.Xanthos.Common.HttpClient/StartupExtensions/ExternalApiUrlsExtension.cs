using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.Common.HttpClient.UrlProvider;

namespace Netsoftware.Xanthos.Common.HttpClient.StartupExtensions;

public static class ExternalApiUrlsExtension
{
    public static IServiceCollection AddExternalApiUrlsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUrlProvider>(new UrlProvider.UrlProvider(configuration));
        services.AddScoped<IHttpClientApiService, HttpClientApiService>();
        return services;
    }
}