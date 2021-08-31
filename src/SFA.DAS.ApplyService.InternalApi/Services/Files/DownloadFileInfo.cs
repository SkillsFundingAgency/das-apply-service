using System;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public class DownloadFileInfo
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }  
        public DateTime CreatedOn { get; set; }
    }
}
