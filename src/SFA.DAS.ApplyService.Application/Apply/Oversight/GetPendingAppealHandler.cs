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
    public class GetPendingAppealHandler : IRequestHandler<GetPendingAppealRequest, PendingAppealOutcomes>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetPendingAppealHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<PendingAppealOutcomes> Handle(GetPendingAppealRequest request, CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetPendingAppealOutcomes(request.SearchTerm, request.SortColumn, request.SortOrder);
        }
    }
}
