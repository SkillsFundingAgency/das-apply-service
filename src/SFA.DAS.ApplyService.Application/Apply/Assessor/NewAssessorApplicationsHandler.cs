using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class NewAssessorApplicationsHandler : IRequestHandler<NewAssessorApplicationsRequest, List<RoatpAssessorApplicationSummary>>
    {
        private readonly IAssessorRepository _repository;

        public NewAssessorApplicationsHandler(IAssessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoatpAssessorApplicationSummary>> Handle(NewAssessorApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetNewAssessorApplications(request.UserId);
        }
    }
}
