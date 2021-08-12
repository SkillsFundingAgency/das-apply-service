using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class GetAppealUploadRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid AppealId { get; set; }
        public Guid AppealUploadId { get; set; }
    }
}
