namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppealUpload
{
    public class GetAppealUploadQueryResult
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}