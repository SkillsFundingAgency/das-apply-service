using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class GetGatewayApplicationsCountsHandler : IRequestHandler<GetGatewayApplicationCountsRequest, GetGatewayApplicationCountsResponse>
    {
        private readonly IApplyRepository _repository;
        public GetGatewayApplicationsCountsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetGatewayApplicationCountsResponse> Handle(GetGatewayApplicationCountsRequest request, CancellationToken cancellationToken)
        {
            var counts = (await _repository.GetGatewayApplicationStatusCounts()).ToList();

            var response = new GetGatewayApplicationCountsResponse
            {
                NewApplicationsCount = counts.Where(x => x.GatewayApplicationStatus == GatewayReviewStatus.New)
                    .Sum(x => x.Count),
                InProgressApplicationsCount = counts.Where(x =>
                        x.GatewayApplicationStatus == GatewayReviewStatus.InProgress ||
                        x.GatewayApplicationStatus == GatewayReviewStatus.ClarificationSent)
                    .Sum(x => x.Count),
                ClosedApplicationsCount = counts.Where(x =>
                        x.GatewayApplicationStatus == GatewayReviewStatus.Fail ||
                        x.GatewayApplicationStatus == GatewayReviewStatus.Pass || 
                        x.GatewayApplicationStatus== GatewayReviewStatus.Reject)
                    .Sum(x => x.Count)
            };

            return response;
        }
    }
}