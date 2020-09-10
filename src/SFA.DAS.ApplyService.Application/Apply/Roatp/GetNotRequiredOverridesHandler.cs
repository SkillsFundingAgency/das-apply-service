using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNotRequiredOverridesHandler : IRequestHandler<GetNotRequiredOverridesRequest, List<NotRequiredOverride>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetNotRequiredOverridesHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<NotRequiredOverride>> Handle(GetNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetNotRequiredOverrides(request.ApplicationId);
        }
    }
}
