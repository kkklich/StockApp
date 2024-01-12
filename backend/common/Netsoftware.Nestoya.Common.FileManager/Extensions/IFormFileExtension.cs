using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Nestoya.Common.FileManager.Extensions
{
    public static class IFormFileExtension
    {
        public static async Task<byte[]> GetBytes(this IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
