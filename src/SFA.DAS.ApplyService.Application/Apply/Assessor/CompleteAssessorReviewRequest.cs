using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CompleteAssessorReviewRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }

        public string Reviewer { get; set; }

        public CompleteAssessorReviewRequest(Guid applicationId, string reviewer)
        {
            ApplicationId = applicationId;
            Reviewer = reviewer;
        }
    }
}
