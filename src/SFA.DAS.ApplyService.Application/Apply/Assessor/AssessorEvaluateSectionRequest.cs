using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{   
    public class AssessorEvaluateSectionRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public bool SectionCompleted { get; set; }
        public string Reviewer;

        public AssessorEvaluateSectionRequest(Guid applicationId, int sequenceId, int sectionId, bool sectionCompleted, string reviewer)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            SectionCompleted = sectionCompleted;
            Reviewer = reviewer;
        }
    }
}
