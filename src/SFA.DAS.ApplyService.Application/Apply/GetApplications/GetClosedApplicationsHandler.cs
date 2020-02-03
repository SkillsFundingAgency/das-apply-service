using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetClosedApplicationsHandler : IRequestHandler<GetClosedApplicationsRequest, IEnumerable<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _applyRepository;
        public GetClosedApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Apply>> Handle(GetClosedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetClosedApplications();
        }
    }
}
