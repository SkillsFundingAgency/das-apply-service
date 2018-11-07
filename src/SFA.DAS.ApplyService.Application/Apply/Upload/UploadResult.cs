using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Upload
{
    public class UploadResult
    {
        public UploadResult()
        {
            Files = new List<UploadedFileResult>();
        }
        public List<UploadedFileResult> Files { get; set; }
    }

    public class UploadedFileResult
    {
        public bool Uploaded { get; set; }
        public string UploadedFileName { get; set; }
        public string QuestionId { get; set; }
    }
}