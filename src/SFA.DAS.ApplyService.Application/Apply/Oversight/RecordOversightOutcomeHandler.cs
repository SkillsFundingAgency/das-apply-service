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

        public async Task<bool> Handle(RecordOversightOutcomeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of {command.OversightStatus} for application Id {command.ApplicationId}");

            var updateOversightStatusResult = await _applyRepository.UpdateOversightReviewStatus(command.ApplicationId, command.OversightStatus,
                                                                                                 command.ApplicationDeterminedDate, command.UserName);

            if (!updateOversightStatusResult)
            {
                return await Task.FromResult(false);
            }

            var applicationStatus = ApplicationReviewStatus.Approved;
            if (command.OversightStatus != OversightReviewStatus.Successful)
            {
                applicationStatus = ApplicationReviewStatus.Declined;
            }

            await _applyRepository.UpdateApplicationStatus(command.ApplicationId, applicationStatus);

            return await Task.FromResult(true);
        }
    }
}
