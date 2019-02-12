using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetOrganisationForApplication
{
    public class GetOrganisationForApplicationHandler : IRequestHandler<GetOrganisationForApplicationRequest, Organisation>
    {
        private readonly IApplyRepository _applyRepository;

        public GetOrganisationForApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Organisation> Handle(GetOrganisationForApplicationRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOrganisationForApplication(request.ApplicationId);
        }
    }
}