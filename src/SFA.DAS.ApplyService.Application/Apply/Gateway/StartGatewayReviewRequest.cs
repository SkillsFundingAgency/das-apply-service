using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class StartGatewayReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public string Reviewer { get; }

        public StartGatewayReviewRequest(Guid applicationId, string reviewer)
        {
            ApplicationId = applicationId;
            Reviewer = reviewer;
        }
    }
}
