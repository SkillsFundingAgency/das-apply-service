using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayPageAnswerPostClarificationRequestHandler : IRequestHandler<UpdateGatewayPageAnswerPostClarificationRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IAuditService _auditService;

        public UpdateGatewayPageAnswerPostClarificationRequestHandler(IApplyRepository applyRepository, IAuditService auditService)
        {
            _applyRepository = applyRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UpdateGatewayPageAnswerPostClarificationRequest request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UpdateGatewayPagePostClarification, request.UserId, request.UserName);

            var application = await _applyRepository.GetApplication(request.ApplicationId);
            var answer = await _applyRepository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);
            var isNew = answer == null;

            if (isNew)
            {
                answer = CreateNewGatewayPageAnswer(request.ApplicationId, request.PageId);
                _auditService.AuditInsert(answer);
            }
            else
            {
                _auditService.AuditUpdate(answer);
            }

            if (answer != null)
            {
                answer.Status = request.Status;
                answer.Comments = request.Comments;
                answer.UpdatedAt = DateTime.UtcNow;
                answer.UpdatedBy = request.UserName;
                answer.ClarificationAnswer = request.ClarificationAnswer;
            }


            if (isNew)
            {
                await _applyRepository.InsertGatewayPageAnswer(answer, request.UserId, request.UserName);
            }
            else
            {
                await _applyRepository.UpdateGatewayPageAnswerPostClarification(answer, request.UserId, request.UserName);
            }

            if (application.GatewayUserId != request.UserId || application.GatewayUserName != request.UserName)
            {
                _auditService.AuditUpdate(application);
                application.GatewayUserId = request.UserId;
                application.GatewayUserName = request.UserName;
                application.UpdatedBy = request.UserName;
                application.UpdatedAt = DateTime.UtcNow;
                await _applyRepository.UpdateApplication(application);
            }

            await _auditService.Save();

            return Unit.Value;
        }

        private GatewayPageAnswer CreateNewGatewayPageAnswer(Guid applicationId, string pageId)
        {
            return new GatewayPageAnswer
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicationId,
                PageId = pageId,
            };
        }
    }
}