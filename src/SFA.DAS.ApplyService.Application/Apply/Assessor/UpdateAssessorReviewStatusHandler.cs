using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class UpdateAssessorReviewStatusHandler : IRequestHandler<UpdateAssessorReviewStatusRequest>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<UpdateAssessorReviewStatusHandler> _logger;

        public UpdateAssessorReviewStatusHandler(IAssessorRepository repository, ILogger<UpdateAssessorReviewStatusHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAssessorReviewStatusRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateAssessorReviewStatus for ApplicationId '{request.ApplicationId}' - Status '{request.Status}'");

            await _repository.UpdateAssessorReviewStatus(request.ApplicationId, request.UserId, request.Status);

            return Unit.Value;
        }
    }
}
