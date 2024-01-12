using Microsoft.AspNetCore.Builder;

namespace Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware.StartupExtensions;

public static class CustomAuthorizationHeaderProviderExtensions
{
    public static IApplicationBuilder UseCustomTokenRequestHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestHandler>();
        return app;
    }
}