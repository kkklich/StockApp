using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware;

public class RequestHandler
{
    private readonly RequestDelegate _next;

    public RequestHandler(RequestDelegate next) => _next = next;

    public Task Invoke(HttpContext httpContext)
    {
        AuthorizationHeaderProvider.SetHeader(
            httpContext.Request.Headers.FirstOrDefault(a => a.Key == "Authorization"));
        return _next(httpContext);
    }
}