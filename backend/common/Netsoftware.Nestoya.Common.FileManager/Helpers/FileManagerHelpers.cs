using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Netsoftware.Nestoya.Common.FileManager.Helpers
{
    public static class FileManagerHelpers
    {
        public const string PDF_TYPE = "application/pdf";

        public static bool CheckFileFormat(IFormFile file, IEnumerable<string> acceptedExtensions)
        {
            string fileFormat = file.FileName.Split('.').Last().ToLower();
            acceptedExtensions = acceptedExtensions.Select(x => x.ToLower());

            return acceptedExtensions.Contains(fileFormat);
        }

        public static bool CheckFileFormat(string extension, IEnumerable<string> acceptedExtensions)
        {
            extension = extension.ToLower();
            acceptedExtensions = acceptedExtensions.Select(x => x.ToLower());

            return acceptedExtensions.Contains(extension);
        }

        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileContentType(string filePath)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(GetFileName(filePath), out string contentType);

            return contentType ?? "application/octet-stream";
        }
    }
}
