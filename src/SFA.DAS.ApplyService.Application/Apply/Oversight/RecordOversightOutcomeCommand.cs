using MediatR;
using System;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public bool? ApproveGateway { get; set; }
        public bool? ApproveModeration { get; set; }
        public OversightReviewStatus OversightStatus { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string InternalComments { get; set; }
        public string ExternalComments { get; set; }
    }
}
