using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class OpenFinancialApplicationsHandler : IRequestHandler<OpenFinancialApplicationsRequest, List<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _repository;

        public OpenFinancialApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Domain.Entities.Apply>> Handle(OpenFinancialApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetOpenFinancialApplications();
        }
    }
}
