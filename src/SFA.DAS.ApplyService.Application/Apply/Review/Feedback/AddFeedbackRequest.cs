using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Feedback
{
    public class AddFeedbackRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string PageId { get; }
        public Domain.Apply.Feedback Feedback { get; }

        public AddFeedbackRequest(Guid applicationId, int sequenceId, int sectionId, string pageId, Domain.Apply.Feedback feedback)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
            Feedback = feedback;
        }
    }
}