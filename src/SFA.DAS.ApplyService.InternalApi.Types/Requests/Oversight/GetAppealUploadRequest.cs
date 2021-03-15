using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight
{
    public class GetAppealUploadRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
        public Guid AppealId { get; set; }
        public Guid AppealUploadId { get; set; }
    }
}
