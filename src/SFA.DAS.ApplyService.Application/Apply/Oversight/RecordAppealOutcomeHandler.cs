using System;
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
    public class RecordAppealOutcomeHandler : IRequestHandler<RecordAppealOutcomeCommand, bool>
    {
        private readonly IApplicationRepository _applyRepository;
        private readonly IAppealRepository _appealRepository;
        private readonly ILogger<RecordOversightOutcomeHandler> _logger;
        private readonly IAuditService _auditService;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;
        private readonly IUnitOfWork _unitOfWork;
        public RecordAppealOutcomeHandler(ILogger<RecordOversightOutcomeHandler> logger,
            IAppealRepository appealRepository,
            IApplicationRepository applyRepository,
            IAuditService auditService,
            IApplicationUpdatedEmailService applicationUpdatedEmailService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _appealRepository = appealRepository;
            _applyRepository = applyRepository;
            _auditService = auditService;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RecordAppealOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Appeal review status of {request.AppealStatus} for application Id {request.ApplicationId}");

            _auditService.StartTracking(UserAction.RecordAppealOutcome, request.UserId, request.UserName);

            var application = await GetApplicationOrThrow(request.ApplicationId);

            var (oversightReview, isNew) = await GetExistingOrNewAppealReview(request.ApplicationId);

            ApplyChanges(oversightReview, request, isNew);
            SaveChanges(oversightReview, application, isNew);

            _auditService.Save();
            await _unitOfWork.Commit();
            
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

        private async Task<(Appeal, bool)> GetExistingOrNewAppealReview(Guid applicationId)
        {
            var isNew = false;
            var appeal = await _appealRepository.GetByApplicationId(applicationId);

            if (appeal == null)
            {
                isNew = true;
                appeal = new Appeal { ApplicationId = applicationId };
                _auditService.AuditInsert(appeal);
            }
            else
            {
                if (appeal.Status != AppealStatus.InProgressOutcome)
                {
                    throw new InvalidOperationException($"Unable to modify appeal review for application {applicationId} with a status of {appeal.Status}");
                }
                _auditService.AuditUpdate(appeal);
            }

            return (appeal, isNew);
        }

        private void ApplyChanges(Appeal appeal, RecordAppealOutcomeCommand request, bool isNew)
        {
            appeal.Status = request.AppealStatus;

            if (!isNew)
            {
                appeal.UpdatedOn = DateTime.UtcNow;
            }

            if (request.AppealStatus == AppealStatus.InProgressOutcome)
            {
                appeal.InProgressDate = DateTime.UtcNow;
                appeal.InProgressInternalComments = request.InternalComments;
                appeal.InProgressExternalComments = request.ExternalComments;
                appeal.InProgressUserId = request.UserId;
                appeal.InProgressUserName = request.UserName;
            }
            else
            {
                appeal.InternalComments = request.InternalComments;
                appeal.ExternalComments = request.ExternalComments;
                appeal.UserId = request.UserId;
                appeal.UserName = request.UserName;
            }
        }

        private void SaveChanges(Appeal appeal, Domain.Entities.Apply application, bool isNew)
        {
            if (isNew)
            {
                _appealRepository.Add(appeal);
            }
            else
            {
                _appealRepository.Update(appeal);
            }

            switch (appeal.Status)
            {
                // case AppealStatus.InProgressOutcome:
                //     application.ApplicationStatus = ApplicationStatus.InProgressOutcome;
                //     break;
                case AppealStatus.Successful:
                case AppealStatus.SuccessfulAlreadyActive:
                case AppealStatus.SuccessfulFitnessForFunding:
                    application.ApplicationStatus = ApplicationStatus.Successful;
                    _applyRepository.Update(application);
                    break;
                // case AppealStatus.Unsuccessful:
                //     application.ApplicationStatus = application.GatewayReviewStatus == GatewayReviewStatus.Rejected
                //         ? ApplicationStatus.Rejected :
                //         ApplicationStatus.Unsuccessful;
               //     break;
            }

            
        }
    }
}