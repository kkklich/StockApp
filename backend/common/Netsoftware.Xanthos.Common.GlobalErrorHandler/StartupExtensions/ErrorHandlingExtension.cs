using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Netsoftware.Xanthos.Common.GlobalErrorHandler.StartupExtensions;

public static class ErrorHandlingExtension
{
    public static void AddGlobalErrorHandling(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var methodName = new StackTrace(contextFeature.Error).GetFrame(0).GetMethod().GetMethodContextName();

                if (contextFeature.Error.GetType() == typeof(UnauthorizedAccessException))
                    context.Response.StatusCode = 401;

                if (contextFeature != null)
                {
                    var error = new
                    {
                        context.Request.Method,
                        context.Request.QueryString.Value,
                        context.Request.Path,
                        context.Response.StatusCode,
                        contextFeature.Error.Message,
                        methodName
                    };

                    logger.LogError($"Request failed {error}");
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                }
            });
        });
    }

    private static string GetMethodContextName(this MemberInfo method)
    {
        if (method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
        {
            var generatedType = method.DeclaringType;
            var originalType = generatedType.DeclaringType;
            var foundMethod = originalType?.GetMethods()
                .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
            return foundMethod?.DeclaringType?.Name + "." + foundMethod?.Name;
        }

        return method.DeclaringType.Name + "." + method.Name;
    }
}