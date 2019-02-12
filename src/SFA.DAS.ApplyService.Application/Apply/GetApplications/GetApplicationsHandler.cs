using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsHandler : IRequestHandler<GetApplicationsRequest, List<Domain.Entities.Application>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<Domain.Entities.Application>> Handle(GetApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetApplications(request.UserId);
        }
    }
}