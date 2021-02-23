using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAndCommentCommandHandler : IRequestHandler<UpdateGatewayReviewStatusAndCommentCommand>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAuditService _auditService;

        public UpdateGatewayReviewStatusAndCommentCommandHandler(IApplyRepository applyRepository,
            IOversightReviewRepository oversightReviewRepository,
            IAuditService auditService)
        {
            _applyRepository = applyRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UpdateGatewayReviewStatusAndCommentCommand request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application == null)
            {
                throw new InvalidOperationException($"Application {request.ApplicationId} not found");
            }
        
            if (application.ApplyData.GatewayReviewDetails != null)
            {
                application.ApplyData.GatewayReviewDetails.OutcomeDateTime = DateTime.UtcNow;
                application.ApplyData.GatewayReviewDetails.Comments = request.GatewayReviewComment;
                application.ApplyData.GatewayReviewDetails.ExternalComments = request.GatewayReviewExternalComment;
            }

            var result = await _applyRepository.UpdateGatewayReviewStatusAndComment(application.ApplicationId, application.ApplyData, request.GatewayReviewStatus, request.UserId, request.UserName);

            if (result && request.GatewayReviewStatus == GatewayReviewStatus.Reject)
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
            }

            return Unit.Value;
        }
    }
}
