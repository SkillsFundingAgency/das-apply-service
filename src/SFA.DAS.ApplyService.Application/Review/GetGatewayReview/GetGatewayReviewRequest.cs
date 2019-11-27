using MediatR;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using System;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayReview
{
    public class GetGatewayReviewRequest : IRequest<Gateway>
    {
        public Guid ApplicationId { get; }

        public GetGatewayReviewRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
