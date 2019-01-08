using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Evaluate
{
    public class EvaluateRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public Domain.Apply.Feedback Feedback { get; }
        public bool IsSectionComplete { get; }

        public EvaluateRequest(Guid applicationId, int sequenceId, int sectionId, Domain.Apply.Feedback feedback, bool isSectionComplete)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            Feedback = feedback;
            IsSectionComplete = isSectionComplete;
        }
    }
}
