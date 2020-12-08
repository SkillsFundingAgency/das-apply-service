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

            var answer = await _applyRepository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);

            if (answer == null)
            {
                answer = new GatewayPageAnswer
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = request.ApplicationId,
                    PageId = request.PageId,
                };

                _auditService.AuditInsert(answer);
            }
            else
            {
                _auditService.AuditUpdate(answer);
            }

            answer.Status = request.Status;
            answer.Comments = request.Comments;
            answer.UpdatedAt = DateTime.UtcNow;
            answer.UpdatedBy = request.UserName;


            //todo: remove the original method in the repo
            //todo: consider splitting into create/update methods:
            await _applyRepository.SubmitGatewayPageAnswer(answer, request.UserId, request.UserName);

            await _auditService.Save();

            return Unit.Value;
        }
    }
}
