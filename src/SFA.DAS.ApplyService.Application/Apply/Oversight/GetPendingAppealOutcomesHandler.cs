using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetPendingAppealOutcomesHandler : IRequestHandler<GetPendingAppealOutcomesRequest, PendingAppealOutcomes>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetPendingAppealOutcomesHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<PendingAppealOutcomes> Handle(GetPendingAppealOutcomesRequest request, CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetPendingAppealOutcomes(request.SearchTerm, request.SortColumn, request.SortOrder);
        }
    }
}