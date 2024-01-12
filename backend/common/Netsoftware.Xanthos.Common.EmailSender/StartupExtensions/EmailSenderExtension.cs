using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.Common.EmailSender.Interfaces;

namespace Netsoftware.Xanthos.Common.EmailSender.StartupExtensions;

public static class EmailSenderExtension
{
    public static IServiceCollection AddEmailSenderModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailConfiguration>(configuration.GetSection("EmailClientConfiguration"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IEmailTemplateGenerator, EmailTemplateGenerator>();
        services.AddScoped<IRazorTemplateGenerator, RazorTemplateGenerator>();
        return services;
    }
}