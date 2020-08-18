using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class ApplicationsInModerationHandler : IRequestHandler<ApplicationsInModerationRequest, List<RoatpModerationApplicationSummary>>
    {
        private readonly IAssessorRepository _repository;

        public ApplicationsInModerationHandler(IAssessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoatpModerationApplicationSummary>> Handle(ApplicationsInModerationRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetApplicationsInModeration();
        }
    }
}
