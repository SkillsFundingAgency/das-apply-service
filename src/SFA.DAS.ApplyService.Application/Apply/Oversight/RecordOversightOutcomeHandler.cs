using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeHandler : IRequestHandler<RecordOversightOutcomeCommand, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<RecordOversightOutcomeHandler> _logger;

        public RecordOversightOutcomeHandler(IApplyRepository applyRepository, ILogger<RecordOversightOutcomeHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RecordOversightOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of {request.OversightStatus} for application Id {request.ApplicationId}");

            var updateOversightStatusResult = await _applyRepository.UpdateOversightReviewStatus(request.ApplicationId, request.OversightStatus,
                                                                                          request.UserId, request.UserName);

            if (!updateOversightStatusResult)
            {
                return false;
            }

            var applicationStatus = ApplicationStatus.Approved;
            if (request.OversightStatus != OversightReviewStatus.Successful)
            {
                applicationStatus = ApplicationStatus.Rejected;
            }

            await _applyRepository.UpdateApplicationStatus(request.ApplicationId, applicationStatus);

            return true;
        }
    }
}
