using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Grade
{
    public class GradeRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string FeedbackComment { get; }
        public bool IsSectionComplete { get; }

        public GradeRequest(Guid applicationId, int sequenceId, int sectionId, string message, bool isSectionComplete)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            FeedbackComment = message;
            IsSectionComplete = isSectionComplete;
        }
    }
}