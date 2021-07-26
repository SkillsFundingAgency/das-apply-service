using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.InternalApi.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<FileUpload> ToFileUpload(this IFormFile formFile)
        {
            return new FileUpload
            {
                Filename = formFile.FileName,
                Data = await formFile.GetBytes(),
                ContentType = formFile.ContentType
            };
        }
    }
}
