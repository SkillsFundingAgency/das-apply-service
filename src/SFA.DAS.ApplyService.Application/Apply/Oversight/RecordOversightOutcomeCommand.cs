using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string OversightStatus { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string InternalComments { get; set; }
        public string ExternalComments { get; set; }
    }
}
