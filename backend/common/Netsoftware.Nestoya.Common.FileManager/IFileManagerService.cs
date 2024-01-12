using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Nestoya.Common.FileManager
{
    public interface IFileManagerService
    {
        Task<FileContentResult> MergeToOnePdfAsync(IEnumerable<string> pdfPaths, string pdfName);
        Task<FileContentResult> CreateZipAsync(IEnumerable<string> filePaths, string zipName);
        Task<FileContentResult> GetFileAsync(string filePath);
        Task<FileContentResult> GetFileAsync(string filePath, string fileName);
        Task<bool> CheckIfFileExist(string filePath);
        Task<bool> CheckIfFileExist(string fileName, string folderName);
        Task<string> SaveFileAsync(IFormFile file, bool keepDuplicate = false, string folderName = "Files");
        Task<string> SaveFileAsync(FileContentResult file, bool keepDuplicate = false, string folderName = "Files");
        Task RemoveFileAsync(string filePath);
        Task RemoveFilePermanentlyAsync(string filePath);
        Task<string> RenameFileAsync(string path, string nameWithoutExtension);
    }
}
