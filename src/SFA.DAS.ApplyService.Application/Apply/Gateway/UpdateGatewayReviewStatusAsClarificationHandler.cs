using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAsClarificationHandler : IRequestHandler<UpdateGatewayReviewStatusAsClarificationRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IAuditService _auditService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGatewayReviewStatusAsClarificationHandler(IApplyRepository applyRepository, IGatewayRepository gatewayRepository,
            IAuditService auditService, IUnitOfWork unitOfWork)
        {
            _applyRepository = applyRepository;
            _gatewayRepository = gatewayRepository;
            _auditService = auditService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateGatewayReviewStatusAsClarificationRequest request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UpdateGatewayReviewStatus, request.UserId, request.UserName);

            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application == null) return false;

            application.ApplicationStatus = ApplicationStatus.Submitted;
            application.GatewayReviewStatus = GatewayReviewStatus.ClarificationSent;

            if (application.ApplyData == null)
                application.ApplyData = new ApplyData();

            if (application.ApplyData.GatewayReviewDetails == null)
            {
                application.ApplyData.GatewayReviewDetails = new ApplyGatewayDetails();
            }

            application.ApplyData.GatewayReviewDetails.ClarificationRequestedOn = DateTime.UtcNow;
            application.ApplyData.GatewayReviewDetails.ClarificationRequestedBy = request.UserId;

            var updatedSuccessfully = await _gatewayRepository.UpdateGatewayReviewStatusAndComment(request.ApplicationId,
                application.ApplyData, application.GatewayReviewStatus, request.UserId, request.UserName);
            _auditService.AuditUpdate(application);

            _auditService.Save();
            await _unitOfWork.Commit();

            return updatedSuccessfully;
        }
    }
}
