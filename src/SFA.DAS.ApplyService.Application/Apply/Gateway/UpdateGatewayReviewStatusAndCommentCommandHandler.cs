using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAndCommentCommandHandler : IRequestHandler<UpdateGatewayReviewStatusAndCommentCommand, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAuditService _auditService;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGatewayReviewStatusAndCommentCommandHandler(IApplyRepository applyRepository,
            IGatewayRepository gatewayRepository,
            IOversightReviewRepository oversightReviewRepository,
            IAuditService auditService,
            IApplicationUpdatedEmailService applicationUpdatedEmailService,
            IUnitOfWork unitOfWork)
        {
            _applyRepository = applyRepository;
            _gatewayRepository = gatewayRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _auditService = auditService;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateGatewayReviewStatusAndCommentCommand request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application?.ApplyData == null)
            {
                throw new InvalidOperationException($"Application {request.ApplicationId} not found");
            }
            else if (application.ApplyData.GatewayReviewDetails == null)
            {
                application.ApplyData.GatewayReviewDetails = new ApplyGatewayDetails();
            }

            application.ApplyData.GatewayReviewDetails.OutcomeDateTime = DateTime.UtcNow;
            application.ApplyData.GatewayReviewDetails.Comments = request.GatewayReviewComment;
            application.ApplyData.GatewayReviewDetails.ExternalComments = request.GatewayReviewExternalComment;

            var updatedSuccessfully = await _gatewayRepository.UpdateGatewayReviewStatusAndComment(application.ApplicationId, application.ApplyData, request.GatewayReviewStatus, request.UserId, request.UserName);

            if (updatedSuccessfully && request.GatewayReviewStatus == GatewayReviewStatus.Reject)
            {
                var oversightReview = new OversightReview
                {
                    ApplicationId = request.ApplicationId,
                    Status = OversightReviewStatus.Rejected,
                    UserId = request.UserId,
                    UserName = request.UserName,
                    InternalComments = request.GatewayReviewComment,
                    ExternalComments = request.GatewayReviewExternalComment
                };

                _auditService.StartTracking(UserAction.UpdateGatewayReviewStatus, request.UserId, request.UserName);
                _auditService.AuditInsert(oversightReview);
                _oversightReviewRepository.Add(oversightReview);
                _auditService.Save();
                await _unitOfWork.Commit();

                await _applicationUpdatedEmailService.SendEmail(request.ApplicationId);
            }

            return updatedSuccessfully;
        }
    }
}
