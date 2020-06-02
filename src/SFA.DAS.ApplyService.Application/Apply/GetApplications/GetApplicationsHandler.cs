using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsHandler : IRequestHandler<GetApplicationsRequest, List<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<Domain.Entities.Apply>> Handle(GetApplicationsRequest request, CancellationToken cancellationToken)
        {
            if(!request.CreatedBy)
            {
                return await _applyRepository.GetOrganisationApplications(request.SigninId);
            }

            return await _applyRepository.GetUserApplications(request.SigninId);
        }
    }
}