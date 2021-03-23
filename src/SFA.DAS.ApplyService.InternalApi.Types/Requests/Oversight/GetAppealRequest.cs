using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight
{
    public class GetAppealRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
    }
}
