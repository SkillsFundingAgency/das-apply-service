namespace SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile
{
    public class GetAppealFileQueryResult
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}