using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Netsoftware.Nestoya.Common.FileManager.Extensions;
using Netsoftware.Nestoya.Common.FileManager.Helpers;
using Netsoftware.Nestoya.Common.FileManager.Services.Interfaces;
using Netsoftware.Xanthos.Common.HttpClient.UrlProvider;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Nestoya.Common.FileManager
{
    public class FileManagerService : IFileManagerService
    {
        private readonly string _storageApiUrl;
        private readonly IFileManagerApiService _fileManagerApiService;
        private readonly IConfiguration _configuration;

        public FileManagerService(IUrlProvider urlProvider, IFileManagerApiService fileManagerApiService, IConfiguration configuration)
        {
            _storageApiUrl = urlProvider.GetUrl("StorageApi");
            _fileManagerApiService = fileManagerApiService;
            _configuration = configuration;
        }

        public async Task<FileContentResult> MergeToOnePdfAsync(IEnumerable<string> pdfPaths, string pdfName)
        {
            if (pdfPaths.Any(x => FileManagerHelpers.GetFileContentType(x) != FileManagerHelpers.PDF_TYPE))
            {
                throw new InvalidOperationException("Incorrect file content type was found within provided pdfPaths");
            }

            return await _fileManagerApiService.GetFile($"{_storageApiUrl}/api/storage/MergeToOnePdf?pathsJSON={JsonConvert.SerializeObject(pdfPaths)}&pdfName={pdfName}");
        }

        public async Task<FileContentResult> CreateZipAsync(IEnumerable<string> filePaths, string zipName)
        {
            return await _fileManagerApiService.GetFile($"{_storageApiUrl}/api/storage/CreateZip?pathsJSON={JsonConvert.SerializeObject(filePaths)}&zipName={zipName}");
        }

        public async Task<FileContentResult> GetFileAsync(string filePath)
        {
            return await _fileManagerApiService.GetFile($"{_storageApiUrl}/api/storage/GetFile?filePath={filePath}");
        }

        public async Task<FileContentResult> GetFileAsync(string filePath, string fileName)
        {
            var result = await _fileManagerApiService.GetFile($"{_storageApiUrl}/api/storage/GetFile?filePath={filePath}");
            result.FileDownloadName = fileName;

            return result;
        }

        /// <summary>
        /// Method returns filePath
        /// </summary>
        public async Task<string> SaveFileAsync(IFormFile file, bool keepDuplicate = false, string folderName = "Files")
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(await file.GetBytes()), "File", file.FileName);

            string filePath = await _fileManagerApiService.PostFile($"{_storageApiUrl}/api/storage/SaveFile?keepDuplicate={keepDuplicate}&folderName={folderName}", formData);
            return filePath;
        }

        /// <summary>
        /// Method returns filePath
        /// </summary>
        public async Task<string> SaveFileAsync(FileContentResult file, bool keepDuplicate = false, string folderName = "Files")
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(file.FileContents), "File", file.FileDownloadName);

            string filePath = await _fileManagerApiService.PostFile($"{_storageApiUrl}/api/storage/SaveFile?keepDuplicate={keepDuplicate}&folderName={folderName}", formData);
            return filePath;
        }

        public async Task RemoveFileAsync(string filePath)
        {
            await _fileManagerApiService.Delete($"{_storageApiUrl}/api/storage/RemoveFile?filePath={filePath}");
        }

        public async Task RemoveFilePermanentlyAsync(string filePath)
        {
            await _fileManagerApiService.Delete($"{_storageApiUrl}/api/storage/RemoveFilePermanently?filePath={filePath}");
        }

        /// <summary>
        /// Use this method when you do not know fully path when the file can be stored, method concats provided fileName and folderName with 
        /// UploadedFilesDirectory from shared settings
        /// </summary>
        public async Task<bool> CheckIfFileExist(string fileName, string folderName)
        {
            string filePath = Path.Combine(_configuration.GetSection("UploadedFilesDirectory").Value, folderName, fileName);
            return await CheckIfFileExist(filePath);
        }

        /// <summary>
        /// Use this method when you know fully path when the file can be stored
        /// </summary>
        public async Task<bool> CheckIfFileExist(string filePath)
        {
            string result = await _fileManagerApiService.GetRaw($"{_storageApiUrl}/api/storage/CheckIfFileExist?filePath={filePath}");
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Method returns filePath
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nameWithoutExtension"></param>
        public async Task<string> RenameFileAsync(string path, string nameWithoutExtension)
        {
            return await _fileManagerApiService.Put($"{_storageApiUrl}/api/storage/RenameFile?filePath={path}&nameWithoutExtension={nameWithoutExtension}", null);
        }
    }
}
