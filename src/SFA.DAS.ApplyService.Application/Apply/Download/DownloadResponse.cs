using System.IO;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class DownloadResponse
    {
        public Stream FileStream { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}