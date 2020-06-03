using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightsPendingHandler : IRequestHandler<GetOversightsPendingRequest, List<ApplicationOversightDetails>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetOversightsPendingHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<ApplicationOversightDetails>> Handle(GetOversightsPendingRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOversightsPending();
        }
    }
}
