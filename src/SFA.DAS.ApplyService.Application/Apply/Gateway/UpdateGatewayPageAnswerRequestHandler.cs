using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayPageAnswerRequestHandler : IRequestHandler<UpdateGatewayPageAnswerRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IAuditService _auditService;

        public UpdateGatewayPageAnswerRequestHandler(IApplyRepository applyRepository, IAuditService auditService)
        {
            _applyRepository = applyRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UpdateGatewayPageAnswerRequest request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UpdateGatewayPageOutcome, request.UserId, request.UserName);

            var application = await _applyRepository.GetApplication(request.ApplicationId);
            var answer = await _applyRepository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);
            var isNew = answer == null;

            if (answer == null)
            {
                answer = CreateNewGatewayPageAnswer(request.ApplicationId, request.PageId);
                _auditService.AuditInsert(answer);
            }
            else
            {
                _auditService.AuditUpdate(answer);
            }

            _auditService.AuditUpdate(application);

            answer.Status = request.Status;
            answer.Comments = request.Comments;
            answer.UpdatedAt = DateTime.UtcNow;
            answer.UpdatedBy = request.UserName;
            application.GatewayUserId = request.UserId;
            application.GatewayUserName = request.UserName;
            application.UpdatedBy = request.UserName;
            application.UpdatedAt = DateTime.UtcNow;

            if (isNew)
            {
                await _applyRepository.InsertGatewayPageAnswer(answer, request.UserId, request.UserName);
            }
            else
            {
                await _applyRepository.UpdateGatewayPageAnswer(answer, request.UserId, request.UserName);
            }

            await _applyRepository.UpdateApplication(application);
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
