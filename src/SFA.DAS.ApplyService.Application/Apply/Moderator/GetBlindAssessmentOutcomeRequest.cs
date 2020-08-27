using MediatR;
using SFA.DAS.ApplyService.InternalApi.Types.Moderator;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetBlindAssessmentOutcomeRequest : IRequest<BlindAssessmentOutcome>
    {
        public GetBlindAssessmentOutcomeRequest(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
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
