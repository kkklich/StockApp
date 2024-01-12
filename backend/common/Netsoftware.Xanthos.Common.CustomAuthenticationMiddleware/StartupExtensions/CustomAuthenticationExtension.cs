using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Netsoftware.Xanthos.Common.Resources.Configurations;

namespace Netsoftware.Xanthos.Common.AuthenticationMiddleware.StartupExtensions;

public static class CustomAuthenticationExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenConfiguration = new TokenConfiguration();
        configuration.GetSection("TokenConfiguration").Bind(tokenConfiguration);

        services.Configure<TokenConfiguration>(configuration.GetSection("TokenConfiguration"));
        services.AddStackExchangeRedisCache(r =>
        {
            r.Configuration = configuration.GetSection("Redis:ConnectionStrings:Tokens").Value;
        });

        var serviceProvider = services.BuildServiceProvider();
        var distributedCache = (IDistributedCache)serviceProvider.GetService(typeof(IDistributedCache));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtAuthentication(tokenConfiguration, distributedCache);
        return services;
    }
}

public static class JwtAuthenticationExtension
{
    public static AuthenticationBuilder AddJwtAuthentication(this AuthenticationBuilder authentication,
        TokenConfiguration tConfig, IDistributedCache distributedCache)
    {
        var key = Encoding.ASCII.GetBytes(tConfig.JwtKey);
        return authentication.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = c => { return Task.CompletedTask; },
                OnChallenge = c =>
                {
                    c.HandleResponse();
                    return Task.CompletedTask;
                },
                OnMessageReceived = c =>
                {
                    if (!IsCurrentActiveToken(c, distributedCache).Result)
                    {
                        c.Fail("Token was deactivated");
                        c.Response.StatusCode = 401;
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = c => { return Task.CompletedTask; }
            };
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = tConfig.TokenValidationConfiguration.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = tConfig.TokenValidationConfiguration.ValidateIssuer,
                ValidateAudience = tConfig.TokenValidationConfiguration.ValidateAudience
            };
        });
    }

    private static async Task<bool> IsCurrentActiveToken(MessageReceivedContext context, IDistributedCache cache)
    {
        return await IsActiveAsync(GetCurrentToken(context), cache);
    }


    private static async Task<bool> IsActiveAsync(string token, IDistributedCache cache)
    {
        return await cache.GetStringAsync(GetKey(token)) == null;
    }

    private static string GetCurrentToken(MessageReceivedContext context)
    {
        var authorizationHeader = context
            .HttpContext.Request.Headers["authorization"];
        return authorizationHeader == StringValues.Empty
            ? string.Empty
            : authorizationHeader.Single().Split(" ").Last();
    }

    private static string GetKey(string token)
    {
        return $"tokens:{token}:deactivated";
    }
}