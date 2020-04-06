using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{ 
    public class StartAssessorSectionReviewRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string Reviewer { get; set; }

        public StartAssessorSectionReviewRequest(Guid applicationId, int sequenceId, int sectionId, string reviewer)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            Reviewer = reviewer;
        }
    }
}
