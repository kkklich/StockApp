using System;
using System.IO;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Netsoftware.Xanthos.Api.Services;
using Netsoftware.Xanthos.Api.StartupExtensions;
using Netsoftware.Xanthos.ApplicationInstance.Api;
using Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware.StartupExtensions;
using Netsoftware.Xanthos.Common.DatabaseConfiguration.StartupExtensions;
using Netsoftware.Xanthos.Common.EmailSender.StartupExtensions;
using Netsoftware.Xanthos.Common.HttpClient.StartupExtensions;
using Netsoftware.Xanthos.Database;
using Netsoftware.Xanthos.Database.Repositories;
using Netsoftware.Xanthos.EmailQueue.Api;
using Netsoftware.Xanthos.EmailQueue.HostService;
using Netsoftware.Xanthos.Infrastructure.Repositories;
using Netsoftware.Xanthos.Infrastructure.Repositories.UnitOfWork;
using Netsoftware.Xanthos.Infrastructure.Services;
using Netsoftware.Xanthos.Infrastructure.Utils.Database;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder();
var parentFolder = Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..");
var sharedSettingsPath = Path.Combine(parentFolder, "sharedsettings.json");

var configuration = builder.Configuration;

configuration
    .AddJsonFile(sharedSettingsPath, true)
    .AddJsonFile("sharedsettings.json", true)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}", true)
    .AddEnvironmentVariables();

var services = builder.Services;
services.AddMvc();

services
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Appstation API", Version = "V1" });
        var filePath = Path.Combine(AppContext.BaseDirectory, "Netsoftware.Xanthos.Api.xml");
        c.IncludeXmlComments(filePath);
    })
    .AddDatabase<ApplicationDbContext>(configuration, "DatabaseAppstation")
    .AddCorsPolicies()
    .AddExternalApiUrlsModule(configuration)
    .AddApplicationInstanceModule()
    .AddEmailSenderModule(configuration)
    .AddEmailQueueModule(configuration)
    .AddEmailQueueHostServiceModule(configuration)
    .AddTransient<IDbInitializer, DbInitializer>()
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
    .AddScoped<ProtoService>()
    .AddScoped<StooqService>()
    .AddScoped<IUnitOfWork, UnitOfWork>()  
    .AddDistributedMemoryCache()
    .AddHttpContextAccessor()
    .AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    );

builder.Logging
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug()
    .AddEventSourceLogger();

if (!builder.Environment.IsDevelopment()) builder.Logging.AddGelf();


var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage().UseCors("DevCorsPolicy");
else app.UseHsts().UseCors("ProdCorsPolicy").UseHttpsRedirection();

app.UseSwagger()
    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "appstation API V1"))
    .UseCustomTokenRequestHandler()   
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints => endpoints.MapControllers());

await app.RunAsync();