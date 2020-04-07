using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetClosedApplicationsHandler : IRequestHandler<GetClosedApplicationsRequest, IEnumerable<RoatpApplicationSummaryItem>>
    {
        private readonly IApplyRepository _applyRepository;
        public GetClosedApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<RoatpApplicationSummaryItem>> Handle(GetClosedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetClosedApplications();
        }
    }
}
