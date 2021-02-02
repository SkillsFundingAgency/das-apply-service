using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeHandler : IRequestHandler<RecordOversightOutcomeCommand, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly ILogger<RecordOversightOutcomeHandler> _logger;
        private readonly IAuditService _auditService;

        public RecordOversightOutcomeHandler(ILogger<RecordOversightOutcomeHandler> logger,
            IOversightReviewRepository oversightReviewRepository,
            IApplyRepository applyRepository,
            IAuditService auditService)
        {
            _logger = logger;
            _oversightReviewRepository = oversightReviewRepository;
            _applyRepository = applyRepository;
            _auditService = auditService;
        }

        public async Task<bool> Handle(RecordOversightOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of {request.OversightStatus} for application Id {request.ApplicationId}");

            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if(application == null)
            {
                throw new InvalidOperationException($"Application {request.ApplicationId} not found");
            }

            var isNew = false;

            var oversightReview = await _oversightReviewRepository.GetByApplicationId(request.ApplicationId);
            if (oversightReview == null)
            {
                isNew = true;
                oversightReview = new OversightReview {ApplicationId = request.ApplicationId};
            }

            _auditService.StartTracking(UserAction.RecordOversightOutcome, request.UserId, request.UserName);

            if (isNew)
            {
                _auditService.AuditInsert(oversightReview);
            }
            else
            {
                _auditService.AuditUpdate(oversightReview);
            }
            
            _auditService.AuditUpdate(application);

            ApplyUserInput(oversightReview, request);

            if (isNew)
            {
                await _oversightReviewRepository.Add(oversightReview);
            }
            else
            {
                await _oversightReviewRepository.Update(oversightReview);
            }

            if (request.OversightStatus != OversightReviewStatus.InProgress)
            {
                application.ApplicationStatus = request.OversightStatus == OversightReviewStatus.Unsuccessful
                    ? ApplicationStatus.Rejected
                    : ApplicationStatus.Approved;

                await _applyRepository.UpdateApplication(application);
            }
            
            await _auditService.Save();

            return true;
        }

        private void ApplyUserInput(OversightReview oversightReview, RecordOversightOutcomeCommand request)
        {
            oversightReview.GatewayApproved = request.ApproveGateway;
            oversightReview.ModerationApproved = request.ApproveModeration;
            oversightReview.Status = request.OversightStatus;
            oversightReview.InternalComments = request.InternalComments;
            oversightReview.ExternalComments = request.ExternalComments;
            oversightReview.UserId = request.UserId;
            oversightReview.UserName = request.UserName;
        }
    }
}
