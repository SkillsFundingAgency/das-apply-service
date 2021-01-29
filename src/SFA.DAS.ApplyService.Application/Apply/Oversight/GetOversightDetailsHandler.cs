using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Types.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
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