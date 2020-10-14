using System.IO;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public class DownloadFile
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }  
    }
}
