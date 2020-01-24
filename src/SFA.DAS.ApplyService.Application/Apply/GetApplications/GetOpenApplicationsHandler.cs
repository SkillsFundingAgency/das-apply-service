using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetOpenApplicationsHandler : IRequestHandler<GetOpenApplicationsRequest, IEnumerable<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _applyRepository;
        public GetOpenApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Apply>> Handle(GetOpenApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOpenApplications();
        }
    }
}
