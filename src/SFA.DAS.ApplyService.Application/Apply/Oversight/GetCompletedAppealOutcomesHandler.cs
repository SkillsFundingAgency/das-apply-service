using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetCompletedAppealOutcomesHandler : IRequestHandler<GetCompletedAppealOutcomesRequest, CompletedAppealOutcomes>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetCompletedAppealOutcomesHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<CompletedAppealOutcomes> Handle(GetCompletedAppealOutcomesRequest request, CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetCompletedAppealOutcomes(request.SearchTerm, request.SortColumn, request.SortOrder);
        }
    }
}