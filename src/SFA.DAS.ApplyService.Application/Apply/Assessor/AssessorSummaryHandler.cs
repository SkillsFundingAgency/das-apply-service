using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorSummaryHandler : IRequestHandler<AssessorSummaryRequest, RoatpAssessorSummary>
    {
        private readonly IApplyRepository _repository;

        public AssessorSummaryHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoatpAssessorSummary> Handle(AssessorSummaryRequest request, CancellationToken cancellationToken)
        {
            var newApplicationCount = _repository.GetNewAssessorApplicationsCount(request.UserId);

            await Task.WhenAll(newApplicationCount);

            return new RoatpAssessorSummary(newApplicationCount.Result, 0, 0, 0);
        }
    }
}
