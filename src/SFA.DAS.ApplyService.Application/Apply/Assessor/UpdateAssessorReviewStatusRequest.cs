using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class UpdateAssessorReviewStatusRequest : IRequest
    {
        public UpdateAssessorReviewStatusRequest(Guid applicationId, string userId, string status)
        {
            ApplicationId = applicationId;
            UserId = userId;
            Status = status;
        }

        public Guid ApplicationId { get; }
        public string UserId { get; }
        public string Status { get; }
    }
}
