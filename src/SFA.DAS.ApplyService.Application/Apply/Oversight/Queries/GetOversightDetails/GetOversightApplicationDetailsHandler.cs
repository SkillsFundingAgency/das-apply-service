using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails
{
    public class GetOversightApplicationDetailsHandler : IRequestHandler<GetOversightApplicationDetailsRequest, ApplicationOversightDetails>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetOversightApplicationDetailsHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<ApplicationOversightDetails> Handle(GetOversightApplicationDetailsRequest request,
            CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetOversightApplicationDetails(request.ApplicationId);
        }
    }
}