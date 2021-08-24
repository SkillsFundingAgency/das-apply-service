using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals
{
    public class GetAppealFileRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
