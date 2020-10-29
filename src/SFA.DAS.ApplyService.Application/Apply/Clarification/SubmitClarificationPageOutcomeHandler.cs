using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class SubmitClarificationPageOutcomeHandler : IRequestHandler<SubmitClarificationPageOutcomeRequest>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<SubmitClarificationPageOutcomeHandler> _logger;

        public SubmitClarificationPageOutcomeHandler(IClarificationRepository repository, ILogger<SubmitClarificationPageOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitClarificationPageOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"SubmitClarificationPageOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - Status '{request.Status}'");

            await _repository.SubmitClarificationPageOutcome(request.ApplicationId, 
                                                        request.SequenceNumber, 
                                                        request.SectionNumber, 
                                                        request.PageId,  
                                                        request.UserId,
                                                        request.Status, 
                                                        request.Comment,
                                                        request.ClarificationResponse,
                                                        request.ClarificationFile);

            return Unit.Value;
        }
    }
}
