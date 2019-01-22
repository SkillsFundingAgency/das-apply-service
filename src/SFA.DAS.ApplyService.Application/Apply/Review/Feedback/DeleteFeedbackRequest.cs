using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Feedback
{
    public class DeleteFeedbackRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string PageId { get; }
        public Guid FeedbackId { get; }

        public DeleteFeedbackRequest(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid feedbackId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
            FeedbackId = feedbackId;
        }
    }
}