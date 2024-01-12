using Microsoft.AspNetCore.Mvc;
using Netsoftware.Xanthos.Common.HttpClient;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Nestoya.Common.FileManager.Services.Interfaces
{
    public interface IFileManagerApiService
    {
        Task<FileContentResult> GetFile(string url, double? timeoutInSeconds = null);
        Task<string> PostFile(string url, MultipartFormDataContent formData, double? timeoutInSeconds = null);
        Task Delete(string url, double? timeoutInSeconds = null);
        Task<string> Put(string url, string body, double? timeoutInSeconds = null);
        Task<string> GetRaw(string url, double? timeoutInSeconds = null);
    }
}
