using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesHandler : IRequestHandler<UpdateNotRequiredOverridesRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        public UpdateNotRequiredOverridesHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<bool> Handle(UpdateNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.UpdateNotRequiredOverrides(request.ApplicationId, request.NotRequiredOverrides);
        }
    }
}
