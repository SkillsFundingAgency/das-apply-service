namespace SFA.DAS.ApplyService.Domain.Models
{
    public class FileUpload
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
