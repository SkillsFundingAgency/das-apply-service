using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class GetAppealRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
    }
}
