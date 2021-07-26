namespace SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight
{
    public class GetAppealUploadResponse
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
