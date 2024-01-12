using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Netsoftware.Nestoya.Common.FileManager.Helpers;
using Netsoftware.Nestoya.Common.FileManager.Services.Interfaces;
using Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware;
using Netsoftware.Xanthos.Common.HttpClient;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Nestoya.Common.FileManager.Services
{
    public class FileManagerApiService : IFileManagerApiService
    {
        protected readonly ILogger _logger;
        private TimeSpan _timeoutInSeconds;
        private readonly IConfiguration _configuration;

        public FileManagerApiService(ILogger<HttpClientApiService> logger, IConfiguration configuration)
        {
            _logger = logger;
            double seconds;
            double.TryParse(configuration.GetSection("RequestsTimeoutInSeconds").Value, out seconds);
            _timeoutInSeconds = TimeSpan.FromSeconds(seconds);
            _configuration = configuration;
        }

        private void SetTimeout(double? timeoutInSeconds, System.Net.Http.HttpClient client)
        {
            if (timeoutInSeconds.HasValue)
            {
                _timeoutInSeconds = TimeSpan.FromSeconds(timeoutInSeconds.Value);
            }
            if (_timeoutInSeconds.TotalSeconds > 0)
            {
                client.Timeout = _timeoutInSeconds;
            }
        }

        /// <summary>
        /// Returns uploaded file path
        /// </summary>
        public async Task<string> PostFile(string url, MultipartFormDataContent formData, double? timeoutInSeconds = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Post,
                        Content = formData
                    };

                    request.Headers.Add("Authorization", new FileManagerToken(_configuration).Token);

                    SetTimeout(timeoutInSeconds, client);

                    var resp = await client.SendAsync(request);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                        var content = await resp.Content?.ReadAsStringAsync();
                        if (content != null)
                        {
                            throw new HttpRequestException(errorMessage, new Exception(content));
                        }
                        throw new HttpRequestException(errorMessage);
                    }

                    return await resp.Content.ReadAsStringAsync();
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error during PostFile method", url, formData, timeoutInSeconds);
                throw e;
            }
        }

        public async Task<string> GetRaw(string url, double? timeoutInSeconds = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("Authorization", new FileManagerToken(_configuration).Token);

                    SetTimeout(timeoutInSeconds, client);

                    var resp = await client.SendAsync(request);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                        var content = await resp.Content?.ReadAsStringAsync();
                        if (content != null)
                        {
                            throw new HttpRequestException(errorMessage, new Exception(content));
                        }
                        throw new HttpRequestException(errorMessage);
                    }

                    return await resp.Content.ReadAsStringAsync();
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error during GetRaw method", url, timeoutInSeconds);
                throw e;
            }
        }

        public async Task<FileContentResult> GetFile(string url, double? timeoutInSeconds = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("Authorization", new FileManagerToken(_configuration).Token);

                    SetTimeout(timeoutInSeconds, client);

                    var resp = await client.SendAsync(request);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                        var content = await resp.Content?.ReadAsStringAsync();
                        if (content != null)
                        {
                            throw new HttpRequestException(errorMessage, new Exception(content));
                        }
                        throw new HttpRequestException(errorMessage);
                    }

                    return new FileContentResult(await resp.Content.ReadAsByteArrayAsync(), resp.Content.Headers.ContentType.MediaType)
                    {
                        FileDownloadName = resp.Content.Headers.ContentDisposition.FileNameStar
                    };
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error during GetFile method", url, timeoutInSeconds);
                throw e;
            }
        }

        public async Task Delete(string url, double? timeoutInSeconds = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Delete,
                    };

                    request.Headers.Add("Authorization", new FileManagerToken(_configuration).Token);

                    SetTimeout(timeoutInSeconds, client);

                    var resp = await client.SendAsync(request);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                        var content = await resp.Content?.ReadAsStringAsync();
                        if (content != null)
                        {
                            throw new HttpRequestException(errorMessage, new Exception(content));
                        }
                        throw new HttpRequestException(errorMessage);
                    }
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error during Delete method", url, timeoutInSeconds);
                throw e;
            }
        }

        public async Task<string> Put(string url, string body, double? timeoutInSeconds = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Put
                    };

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    request.Headers.Add("Authorization", new FileManagerToken(_configuration).Token);

                    SetTimeout(timeoutInSeconds, client);

                    var resp = await client.SendAsync(request);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errorMessage = $"Error while calling : {url}, response StatusCode = {resp.StatusCode}.";
                        var content = await resp.Content?.ReadAsStringAsync();
                        if (content != null)
                        {
                            throw new HttpRequestException(errorMessage, new Exception(content));
                        }
                        throw new HttpRequestException(errorMessage);
                    }

                   return await resp.Content.ReadAsStringAsync();
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error during Put method", url, timeoutInSeconds);
                throw;
            }

        }
    }
}
