using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netsoftware.Nestoya.Common.FileManager.Services;
using Netsoftware.Nestoya.Common.FileManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Netsoftware.Nestoya.Common.FileManager.StartupExtensions
{
    public static class FileManagerExtension
    {
        public static void AddFileManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileManagerService, FileManagerService>();
            services.AddScoped<IFileManagerApiService, FileManagerApiService>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
