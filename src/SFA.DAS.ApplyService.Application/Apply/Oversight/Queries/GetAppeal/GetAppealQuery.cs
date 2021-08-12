using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppeal
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class GetAppealQuery : IRequest<GetAppealQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
    }
}
