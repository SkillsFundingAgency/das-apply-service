using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightsCompletedHandler : IRequestHandler<GetOversightsCompletedRequest, List<ApplicationOversightDetails>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetOversightsCompletedHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<ApplicationOversightDetails>> Handle(GetOversightsCompletedRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOversightsCompleted();
        }
    }
}
