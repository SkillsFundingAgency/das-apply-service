using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Types.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightsCompletedHandler : IRequestHandler<GetOversightsCompletedRequest, CompletedOversightReviews>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetOversightsCompletedHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<CompletedOversightReviews> Handle(GetOversightsCompletedRequest request, CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetCompletedOversightReviews();
        }
    }
}
