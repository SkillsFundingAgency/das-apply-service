﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightOutcomeHandler : IRequestHandler<RecordOversightOutcomeCommand, bool>
    {
        private readonly IApplicationRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly ILogger<RecordOversightOutcomeHandler> _logger;
        private readonly IAuditService _auditService;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;
        private readonly IUnitOfWork _unitOfWork;
        public RecordOversightOutcomeHandler(ILogger<RecordOversightOutcomeHandler> logger,
            IOversightReviewRepository oversightReviewRepository,
            IApplicationRepository applyRepository,
            IAuditService auditService,
            IApplicationUpdatedEmailService applicationUpdatedEmailService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _oversightReviewRepository = oversightReviewRepository;
            _applyRepository = applyRepository;
            _auditService = auditService;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RecordOversightOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of {request.OversightStatus} for application Id {request.ApplicationId}");

            _auditService.StartTracking(UserAction.RecordOversightOutcome, request.UserId, request.UserName);

            var application = await GetApplicationOrThrow(request.ApplicationId);

            var (oversightReview, isNew) = await GetExistingOrNewOversightReview(request.ApplicationId);

            ApplyChanges(oversightReview, request, isNew);
            SaveChanges(oversightReview, application, isNew);

            _auditService.Save();
            await _unitOfWork.Commit();
            await _applicationUpdatedEmailService.SendEmail(request.ApplicationId);

            return true;
        }

        private async Task<Domain.Entities.Apply> GetApplicationOrThrow(Guid applicationId)
        {
            var application = await _applyRepository.GetApplication(applicationId);

            if (application == null)
            {
                throw new InvalidOperationException($"Application {applicationId} not found");
            }

            _auditService.AuditUpdate(application);

            return application;
        }

        private async Task<(OversightReview, bool)> GetExistingOrNewOversightReview(Guid applicationId)
        {
            var isNew = false;
            var oversightReview = await _oversightReviewRepository.GetByApplicationId(applicationId);

            if (oversightReview == null)
            {
                isNew = true;
                oversightReview = new OversightReview { ApplicationId = applicationId };
                _auditService.AuditInsert(oversightReview);
            }
            else
            {
                if (oversightReview.Status != OversightReviewStatus.InProgress)
                {
                    throw new InvalidOperationException($"Unable to modify oversight review for application {applicationId} with a status of {oversightReview.Status}");
                }
                _auditService.AuditUpdate(oversightReview);
            }

            return (oversightReview, isNew);
        }

        private void ApplyChanges(OversightReview oversightReview, RecordOversightOutcomeCommand request, bool isNew)
        {
            oversightReview.GatewayApproved = request.ApproveGateway;
            oversightReview.ModerationApproved = request.ApproveModeration;
            oversightReview.Status = request.OversightStatus;

            if (!isNew)
            {
                oversightReview.UpdatedOn = DateTime.UtcNow;
            }

            if (request.OversightStatus == OversightReviewStatus.InProgress)
            {
                oversightReview.InProgressDate = DateTime.UtcNow;
                oversightReview.InProgressInternalComments = request.InternalComments;
                oversightReview.InProgressExternalComments = request.ExternalComments;
                oversightReview.InProgressUserId = request.UserId;
                oversightReview.InProgressUserName = request.UserName;
            }
            else
            {
                oversightReview.InternalComments = request.InternalComments;
                oversightReview.ExternalComments = request.ExternalComments;
                oversightReview.UserId = request.UserId;
                oversightReview.UserName = request.UserName;
            }
        }

        private void SaveChanges(OversightReview oversightReview, Domain.Entities.Apply application, bool isNew)
        {
            if (isNew)
            {
                _oversightReviewRepository.Add(oversightReview);
            }
            else
            {
                _oversightReviewRepository.Update(oversightReview);
            }

            switch (oversightReview.Status)
            {
                case OversightReviewStatus.InProgress:
                    application.ApplicationStatus = ApplicationStatus.InProgressOutcome;
                    break;
                case OversightReviewStatus.Successful:
                case OversightReviewStatus.SuccessfulAlreadyActive:
                case OversightReviewStatus.SuccessfulFitnessForFunding:
                    application.ApplicationStatus = ApplicationStatus.Successful;
                    break;
                case OversightReviewStatus.Unsuccessful:
                    application.ApplicationStatus = application.GatewayReviewStatus == GatewayReviewStatus.Rejected 
                        ? ApplicationStatus.Rejected : 
                        ApplicationStatus.Unsuccessful;
                    break;
            }

            _applyRepository.Update(application);
        }
    }
}
