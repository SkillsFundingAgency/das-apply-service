using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorSummaryHandler : IRequestHandler<AssessorSummaryRequest, RoatpAssessorSummary>
    {
        private readonly IAssessorRepository _repository;

        public AssessorSummaryHandler(IAssessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoatpAssessorSummary> Handle(AssessorSummaryRequest request, CancellationToken cancellationToken)
        {
            var newApplicationCount = _repository.GetNewAssessorApplicationsCount(request.UserId);
            var inProgressApplicationCount = _repository.GetInProgressAssessorApplicationsCount(request.UserId);
            var inModerationApplicationCount = _repository.GetApplicationsInModerationCount();

            await Task.WhenAll(newApplicationCount, inProgressApplicationCount, inModerationApplicationCount);

            return new RoatpAssessorSummary(newApplicationCount.Result, inProgressApplicationCount.Result, inModerationApplicationCount.Result, 0);
        }
    }
}
