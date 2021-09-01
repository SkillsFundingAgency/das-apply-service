using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetCompletedAppealOutcomeHandler : IRequestHandler<GetCompletedAppealRequest, CompletedAppealOutcomes>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetCompletedAppealOutcomeHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<CompletedAppealOutcomes> Handle(GetCompletedAppealRequest request, CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetCompletedAppealOutcomes(request.SearchTerm, request.SortColumn, request.SortOrder);
        }
    }
}
