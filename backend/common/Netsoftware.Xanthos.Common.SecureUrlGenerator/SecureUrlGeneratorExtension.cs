using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Xanthos.Common.UrlTokenGenerator;
using Netsoftware.Xanthos.Common.UrlTokenGenerator.Configuration;
using Netsoftware.Xanthos.Common.UrlTokenGenerator.Implementation;

namespace Netsoftware.Xanthos.Common.SecureUrlGenerator;

public static class SecureUrlGeneratorExtension
{
    public static void UseSecureUrlGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        var secureUrlConfiguration = new SecureUrlGeneratorConfiguration();
        configuration.GetSection("SecureUrlGeneratorConfiguration").Bind(secureUrlConfiguration);

        services.AddSingleton<ISecureStringProvider, SecureStringProvider>();
        services.AddScoped<ISecureUrlProvider, SecureUrlProvider>();
        services.AddScoped<ISecureUrlDecoder, SecureUrlDecoder>();
        services.AddDataProtection()
            .PersistKeysToFileSystem(
                new DirectoryInfo(secureUrlConfiguration.PersistKeysToFileSystem))
            .SetApplicationName(secureUrlConfiguration.ProtectorApplicationName)
            .ProtectKeysWithCertificate(new X509Certificate2(secureUrlConfiguration.KeyCertificateFileName,
                secureUrlConfiguration.KeyCertificatePassword))
            .DisableAutomaticKeyGeneration();
    }
}