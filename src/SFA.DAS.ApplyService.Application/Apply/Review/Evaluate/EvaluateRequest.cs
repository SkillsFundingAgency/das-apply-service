using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Evaluate
{
    public class EvaluateRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string FeedbackComment { get; }
        public bool IsSectionComplete { get; }

        public EvaluateRequest(Guid applicationId, int sequenceId, int sectionId, string message, bool isSectionComplete)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            FeedbackComment = message;
            IsSectionComplete = isSectionComplete;
        }
    }
}
