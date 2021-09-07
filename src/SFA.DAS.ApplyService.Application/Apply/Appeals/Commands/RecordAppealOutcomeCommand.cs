using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Commands
{
    public class RecordAppealOutcomeCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string AppealStatus { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string InternalComments { get; set; }
        public string ExternalComments { get; set; }
    }
}