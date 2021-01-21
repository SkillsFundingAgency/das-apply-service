using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeHandler : IRequestHandler<RecordOversightOutcomeCommand, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly ILogger<RecordOversightOutcomeHandler> _logger;

        public RecordOversightOutcomeHandler(ILogger<RecordOversightOutcomeHandler> logger, IOversightReviewRepository oversightReviewRepository, IApplyRepository applyRepository)
        {
            _logger = logger;
            _oversightReviewRepository = oversightReviewRepository;
            _applyRepository = applyRepository;
        }

        public async Task<bool> Handle(RecordOversightOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of {request.OversightStatus} for application Id {request.ApplicationId}");

            var oversightReview = new OversightReview
            {
                ApplicationId = request.ApplicationId,
                Status = request.OversightStatus,
                ApplicationDeterminedDate = DateTime.UtcNow.Date,
                InternalComments = request.InternalComments,
                ExternalComments = request.ExternalComments,
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _oversightReviewRepository.Add(oversightReview);

            var applicationStatus = ApplicationStatus.Approved;
            if (request.OversightStatus == OversightReviewStatus.Unsuccessful)
            {
                applicationStatus = ApplicationStatus.Rejected;
            }

            await _applyRepository.UpdateApplicationStatus(request.ApplicationId, applicationStatus);

            return true;
        }
    }
}
