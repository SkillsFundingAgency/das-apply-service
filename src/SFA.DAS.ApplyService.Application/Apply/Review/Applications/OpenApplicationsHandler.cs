using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class OpenApplicationsHandler : IRequestHandler<OpenApplicationsRequest, List<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _repository;

        public OpenApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Domain.Entities.Apply>> Handle(OpenApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetOpenApplications();
        }
    }
}
