using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals
{
    public class GetAppealFileRequest
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
    }
}
