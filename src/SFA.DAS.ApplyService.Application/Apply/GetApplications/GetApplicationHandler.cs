using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationHandler : IRequestHandler<GetApplicationRequest, Domain.Entities.Application>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Domain.Entities.Application> Handle(GetApplicationRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetApplication(request.ApplicationId);
        }
    }
}