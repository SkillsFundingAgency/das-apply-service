using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands;
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
        private readonly ILogger<RecordAppealOutcomeHandler> _logger;
        private readonly IAuditService _auditService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;
        public RecordAppealOutcomeHandler(ILogger<RecordAppealOutcomeHandler> logger,
            IAppealRepository appealRepository,
            IApplicationRepository applyRepository,
            IAuditService auditService, 
            IUnitOfWork unitOfWork,
            IApplicationUpdatedEmailService applicationUpdatedEmailService)
        {
            _logger = logger;
            _appealRepository = appealRepository;
            _applyRepository = applyRepository;
            _auditService = auditService;
            _unitOfWork = unitOfWork;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
        }

        public async Task<bool> Handle(RecordAppealOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Appeal review status of {request.AppealStatus} for application Id {request.ApplicationId}");

            _auditService.StartTracking(UserAction.RecordAppealOutcome, request.UserId, request.UserName);

            var application = await GetApplicationOrThrow(request.ApplicationId);

            var (appeal, isNew) = await GetExistingAppeal(request.ApplicationId);

            ApplyChanges(appeal, request, isNew);
            SaveChanges(appeal, application, isNew);

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

         

            return application;
        }

        private async Task<(Appeal, bool)> GetExistingAppeal(Guid applicationId)
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
                if (appeal.Status != AppealStatus.InProgress && appeal.Status != AppealStatus.Submitted) 
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

            if (request.AppealStatus == AppealStatus.InProgress)
            {
                appeal.InProgressDate = DateTime.UtcNow;
                appeal.InProgressInternalComments = request.InternalComments;
                appeal.InProgressExternalComments = request.ExternalComments;
                appeal.InProgressUserId = request.UserId;
                appeal.InProgressUserName = request.UserName;
            }
           

            if(request.AppealStatus==AppealStatus.Successful
                || request.AppealStatus==AppealStatus.Unsuccessful
                || request.AppealStatus==AppealStatus.SuccessfulFitnessForFunding
                || request.AppealStatus==AppealStatus.SuccessfulAlreadyActive)
            {
                appeal.AppealDeterminedDate = DateTime.UtcNow;
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

            if (!(application.ApplicationStatus == ApplicationStatus.Unsuccessful && appeal.Status == AppealStatus.Unsuccessful))
            {
                _auditService.AuditUpdate(application);
            }
            
            switch (appeal.Status)
            {
                case AppealStatus.InProgress:
                    application.ApplicationStatus = ApplicationStatus.InProgressAppeal;
                    break;
                case AppealStatus.Successful:
                case AppealStatus.SuccessfulAlreadyActive:
                case AppealStatus.SuccessfulFitnessForFunding:
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Fail)
                        application.ApplicationStatus = ApplicationStatus.AppealSuccessful;
                            else
                        application.ApplicationStatus = ApplicationStatus.Successful;
                    break;
                case AppealStatus.Unsuccessful:
                    application.ApplicationStatus = ApplicationStatus.Unsuccessful;
                    break;
            }

            _applyRepository.Update(application);
            _applicationUpdatedEmailService.SendEmail(appeal.ApplicationId);
        }
    }
}