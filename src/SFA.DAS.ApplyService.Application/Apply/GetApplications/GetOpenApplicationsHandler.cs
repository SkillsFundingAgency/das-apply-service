using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetOpenApplicationsHandler : IRequestHandler<GetOpenApplicationsRequest, IEnumerable<RoatpApplicationSummaryItem>>
    {
        private readonly IApplyRepository _applyRepository;
        public GetOpenApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<RoatpApplicationSummaryItem>> Handle(GetOpenApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOpenApplications();
        }
    }
}
