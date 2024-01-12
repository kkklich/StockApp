using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Common.EmailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string Generate<TModel>(string rootpath, string templatename, string templatekey, TModel model);
    Task<string> GenerateWithRazorLightAsync<TModel>(string absoluteRootPath, string templatename, TModel model);
}