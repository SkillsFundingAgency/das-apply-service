using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorApplicationCountsHandler : IRequestHandler<AssessorApplicationCountsRequest, RoatpAssessorApplicationCounts>
    {
        private readonly IAssessorRepository _repository;

        public AssessorApplicationCountsHandler(IAssessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoatpAssessorApplicationCounts> Handle(AssessorApplicationCountsRequest request, CancellationToken cancellationToken)
        {
            var newApplicationCount = _repository.GetNewAssessorApplicationsCount(request.UserId);
            var inProgressApplicationCount = _repository.GetInProgressAssessorApplicationsCount(request.UserId);
            var inModerationApplicationCount = _repository.GetApplicationsInModerationCount();

            await Task.WhenAll(newApplicationCount, inProgressApplicationCount, inModerationApplicationCount);

            return new RoatpAssessorApplicationCounts(newApplicationCount.Result, inProgressApplicationCount.Result, inModerationApplicationCount.Result, 0);
        }
    }
}
