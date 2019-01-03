using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.CompleteSection
{
    public class CompleteSectionRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string FeedbackComment { get; }
        public bool IsSectionComplete { get; }

        public CompleteSectionRequest(Guid applicationId, int sequenceId, int sectionId, string message, bool isSectionComplete)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            FeedbackComment = message;
            IsSectionComplete = isSectionComplete;
        }
    }
}
