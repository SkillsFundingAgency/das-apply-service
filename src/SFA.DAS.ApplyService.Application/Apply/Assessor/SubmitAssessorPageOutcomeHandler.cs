using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class SubmitAssessorPageOutcomeHandler : IRequestHandler<SubmitAssessorPageOutcomeRequest, Unit>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<SubmitAssessorPageOutcomeHandler> _logger;

        public SubmitAssessorPageOutcomeHandler(IAssessorRepository repository, ILogger<SubmitAssessorPageOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitAssessorPageOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SubmitAssessorPageOutcome for ApplicationId '{ApplicationId}' - PageId '{PageId}' - Status '{Status}'", request.ApplicationId, request.PageId, request.Status);

            await _repository.SubmitAssessorPageOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId,
                                                        request.UserId,
                                                        request.UserName,
                                                        request.Status,
                                                        request.Comment);

            return Unit.Value;
        }
    }
}
