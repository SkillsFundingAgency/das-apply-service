using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class FinancialApplicationsStatusCountsHandler : IRequestHandler<FinancialApplicationsStatusCountsRequest, RoatpFinancialApplicationsStatusCounts>
    {
        private readonly IApplyRepository _repository;

        public FinancialApplicationsStatusCountsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoatpFinancialApplicationsStatusCounts> Handle(FinancialApplicationsStatusCountsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetFinancialApplicationsStatusCounts();
        }
    }
}
