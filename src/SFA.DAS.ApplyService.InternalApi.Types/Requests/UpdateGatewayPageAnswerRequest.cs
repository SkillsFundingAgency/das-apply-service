using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests
{
    public class UpdateGatewayPageAnswerRequest
    {
        public Guid ApplicationId { get; set; }
        public string PageId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
