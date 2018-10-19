using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public StartApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var workflow = await _applyRepository.GetCurrentWorkflow(request.ApplicationType, request.ApplyingOrganisationId);

            await _applyRepository.SetOrganisationApplication(workflow, request.ApplyingOrganisationId, request.Username);
            
            return Unit.Value;
        }
    }
}