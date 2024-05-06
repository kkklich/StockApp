using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware;
using Newtonsoft.Json;

namespace Netsoftware.Xanthos.Common.HttpClient;

public class HttpClientApiService : IHttpClientApiService
{
    private readonly ILogger _logger;
    private TimeSpan _timeoutInSeconds;

    public HttpClientApiService(ILogger<HttpClientApiService> logger, IConfiguration configuration)
    {
        _logger = logger;
        double.TryParse(configuration.GetSection("RequestsTimeoutInSeconds").Value, out var seconds);
        _timeoutInSeconds = TimeSpan.FromSeconds(seconds);
    }

    public async Task<TOutput> Get<TOutput>(string url, bool withToken = true, double? timeoutInSeconds = null)
        where TOutput : class
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            var result = await resp.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TOutput>(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error during execute Get() method. Expected response data type: {typeof(TOutput)}, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<HttpResponseMessage> GetRaw(string url, bool withToken = true, double? timeoutInSeconds = null)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during execute GetRaw() method, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<TOutput> Post<TOutput>(string url, string body, bool withToken = true,
        double? timeoutInSeconds = null) where TOutput : class
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage =
                    $"Error while calling : {url}, with body : {body}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            var result = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOutput>(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error during execute Post() method. Expected response data type: {typeof(TOutput)}, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<HttpResponseMessage> PostRaw(string url, string body, bool withToken = true,
        double? timeoutInSeconds = null)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during execute PostRaw() method, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task Delete(string url, bool withToken = true, double? timeoutInSeconds = null)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Delete
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during execute Delete() method. Url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<FileContentResult> GetFile(string url, bool withToken = true, double? timeoutInSeconds = null)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            SetTimeout(timeoutInSeconds, client);

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            return new FileContentResult(await resp.Content.ReadAsByteArrayAsync(),
                resp.Content.Headers.ContentType?.MediaType)
            {
                FileDownloadName = resp.Content.Headers.ContentDisposition.FileNameStar
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error during execute GetFile() method. Expected response data type: {typeof(FileContentResult)}, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<TOutput> PostFile<TOutput>(string url, MultipartFormDataContent formData, bool withToken = true)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = formData
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            var result = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOutput>(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during PostFile method", url, formData);
            throw;
        }
    }

    public async Task<TOutput> Patch<TOutput>(string url, string body, bool withToken = true)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Patch
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            var result = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOutput>(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error during execute Post() method. Expected response data type: {typeof(TOutput)}, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<HttpResponseMessage> PatchRaw(string url, string body, bool withToken = true)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Patch
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during execute PostRaw() method, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    public async Task<HttpResponseMessage> PutRaw(string url, string body, bool withToken = true)
    {
        try
        {
            using var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Put
            };

            if (withToken)
            {
                var authorizationHeader = AuthorizationHeaderProvider.GetAuthorizationHeader();
                request.Headers.Add(authorizationHeader.Key, authorizationHeader.Value.ToString());
            }

            var resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                var content = await resp.Content?.ReadAsStringAsync();
                if (content != null) throw new HttpRequestException(errorMessage, new Exception(content));
                throw new HttpRequestException(errorMessage);
            }

            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during execute PutRaw() method, url:{url}, with token: {withToken}.");
            throw;
        }
    }

    private void SetTimeout(double? timeoutInSeconds, System.Net.Http.HttpClient client)
    {
        if (timeoutInSeconds.HasValue) _timeoutInSeconds = TimeSpan.FromSeconds(timeoutInSeconds.Value);
        if (_timeoutInSeconds.TotalSeconds > 0) client.Timeout = _timeoutInSeconds;
    }
}