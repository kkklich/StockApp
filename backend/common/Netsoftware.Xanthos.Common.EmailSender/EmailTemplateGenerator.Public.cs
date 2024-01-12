using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.EmailSender.Interfaces;
using RazorEngine;
using RazorEngine.Templating;
using RazorLight;

namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailTemplateGenerator : IEmailTemplateGenerator
{
    private readonly ILogger _logger;

    public EmailTemplateGenerator(ILogger<EmailTemplateGenerator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Generate an HTML document from the specified Razor template and model.
    /// </summary>
    /// <param name="rootpath">The path to the folder containing the Razor templates</param>
    /// <param name="templatename">The name of the Razor template (.cshtml)</param>
    /// <param name="templatekey">The template key used for caching Razor templates which is essential for improved performance</param>
    /// <param name="model">The model containing the information to be supplied to the Razor template</param>
    /// <returns></returns>
    public string Generate<TModel>(string rootpath, string templatename, string templatekey, TModel model)
    {
        try
        {
            var result = string.Empty;

            if (string.IsNullOrEmpty(rootpath) || string.IsNullOrEmpty(templatename) || model == null) return result;

            var templateFilePath = Path.Combine(rootpath, templatename);

            if (File.Exists(templateFilePath))
            {
                var template = File.ReadAllText(templateFilePath);

                if (string.IsNullOrEmpty(templatekey)) templatekey = Guid.NewGuid().ToString();
                result = Engine.Razor.RunCompile(template, templatekey, typeof(TModel), model);
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error during execute method Generate. Error details: {e}");
            throw;
        }
    }

    /// <summary>
    ///     Generate using RazorLight in case Generate<TModel> method that use RazorEngine not works
    /// </summary>
    /// <param name="absoluteRootPath">Absolute path to the folder containing the Razor templates</param>
    /// <param name="templatename">The name of the Razor template (.cshtml)</param>
    /// <param name="model">The model containing the information to be supplied to the Razor template</param>
    public async Task<string> GenerateWithRazorLightAsync<TModel>(string absoluteRootPath, string templatename,
        TModel model)
    {
        try
        {
            var result = string.Empty;

            if (string.IsNullOrEmpty(absoluteRootPath) || string.IsNullOrEmpty(templatename) || model == null)
                return result;

            var templateFilePath = Path.Combine(absoluteRootPath, templatename);

            if (File.Exists(templateFilePath))
            {
                var engine = new RazorLightEngineBuilder()
                    .UseFileSystemProject(absoluteRootPath)
                    .UseMemoryCachingProvider()
                    .Build();

                result = await engine.CompileRenderAsync(templatename, model);
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error during execute method Generate. Error details: {e}");
            throw;
        }
    }
}