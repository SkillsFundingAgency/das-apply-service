using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string OversightStatus { get; set; }
        public DateTime ApplicationDeterminedDate { get; set; }
        public string UserName { get; set; }

        public RecordOversightOutcomeCommand()
        {

        }

        public RecordOversightOutcomeCommand(Guid applicationId, string oversightStatus, DateTime applicationDeterminedDate, string userName)
        {
            ApplicationId = applicationId;
            OversightStatus = oversightStatus;
            ApplicationDeterminedDate = applicationDeterminedDate;
            UserName = userName;
        }
    }
}
