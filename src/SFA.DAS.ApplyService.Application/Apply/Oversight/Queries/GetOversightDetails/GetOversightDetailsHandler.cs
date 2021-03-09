using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails
{
    public class GetOversightDetailsHandler : IRequestHandler<GetOversightDetailsRequest, ApplicationOversightDetails>
    {
        private readonly IOversightReviewQueries _oversightReviewQueries;

        public GetOversightDetailsHandler(IOversightReviewQueries oversightReviewQueries)
        {
            _oversightReviewQueries = oversightReviewQueries;
        }

        public async Task<ApplicationOversightDetails> Handle(GetOversightDetailsRequest request,
            CancellationToken cancellationToken)
        {
            return await _oversightReviewQueries.GetOversightDetails(request.ApplicationId);
        }
    }
}