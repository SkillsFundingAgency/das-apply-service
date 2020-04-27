using System.IO;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class DownloadFile
    {
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
