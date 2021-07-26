using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview
{
    public class GetOversightReviewQuery : IRequest<GetOversightReviewQueryResult>
    {
        public Guid ApplicationId { get; set; }
    }
}
