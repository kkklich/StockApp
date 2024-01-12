using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Netsoftware.Xanthos.Common.HttpClient;

public interface IHttpClientApiService
{
    Task Delete(string url, bool withToken = true, double? timeoutInSeconds = null);

    Task<TOutput> Get<TOutput>(string url, bool withToken = true, double? timeoutInSeconds = null)
        where TOutput : class;

    Task<HttpResponseMessage> GetRaw(string url, bool withToken = true, double? timeoutInSeconds = null);

    Task<TOutput> Post<TOutput>(string url, string body, bool withToken = true, double? timeoutInSeconds = null)
        where TOutput : class;

    Task<HttpResponseMessage> PostRaw(string url, string body, bool withToken = true, double? timeoutInSeconds = null);
    Task<FileContentResult> GetFile(string url, bool withToken = true, double? timeoutInSeconds = null);
    Task<TOutput> PostFile<TOutput>(string url, MultipartFormDataContent formData, bool withToken = true);
    Task<TOutput> Patch<TOutput>(string url, string body, bool withToken = true);
    Task<HttpResponseMessage> PatchRaw(string url, string body, bool withToken = true);
    Task<HttpResponseMessage> PutRaw(string url, string body, bool withToken = true);
}