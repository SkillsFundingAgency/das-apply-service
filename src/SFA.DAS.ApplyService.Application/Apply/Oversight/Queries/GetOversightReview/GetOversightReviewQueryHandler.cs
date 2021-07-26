using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview
{
    public class GetOversightReviewQueryHandler : IRequestHandler<GetOversightReviewQuery, GetOversightReviewQueryResult>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetOversightReviewQueryHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<GetOversightReviewQueryResult> Handle(GetOversightReviewQuery request, CancellationToken cancellationToken)
        {
            var result = await _oversightReviewQueries.GetOversightReview(request.ApplicationId);

            return result == null ? null : new GetOversightReviewQueryResult
            {
                Id = result.Id,
                Status = result.Status,
                ApplicationDeterminedDate = result.ApplicationDeterminedDate,
                InProgressDate = result.InProgressDate,
                InProgressUserId = result.InProgressUserId,
                InProgressUserName = result.InProgressUserName,
                InProgressInternalComments = result.InProgressInternalComments,
                InProgressExternalComments = result.InProgressExternalComments,
                GatewayApproved = result.GatewayApproved,
                ModerationApproved = result.ModerationApproved,
                InternalComments = result.InternalComments,
                ExternalComments = result.ExternalComments,
                UserId = result.UserId,
                UserName = result.UserName
            };
        }
    }
}