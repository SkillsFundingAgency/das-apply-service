using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetModerationOutcomeRequest : IRequest<ModerationOutcome>
    {
        public GetModerationOutcomeRequest(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
    }
}
