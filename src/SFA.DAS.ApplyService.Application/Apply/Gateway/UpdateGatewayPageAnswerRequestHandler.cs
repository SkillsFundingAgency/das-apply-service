﻿using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayPageAnswerRequestHandler : IRequestHandler<UpdateGatewayPageAnswerRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IAuditService _auditService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGatewayPageAnswerRequestHandler(IApplyRepository applyRepository, IGatewayRepository gatewayRepository, IAuditService auditService, IUnitOfWork unitOfWork)
        {
            _applyRepository = applyRepository;
            _gatewayRepository = gatewayRepository;
            _auditService = auditService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateGatewayPageAnswerRequest request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UpdateGatewayPageOutcome, request.UserId, request.UserName);

            var application = await _applyRepository.GetApplication(request.ApplicationId);
            var answer = await _gatewayRepository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);
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
                if (string.IsNullOrEmpty(answer.ClarificationAnswer))
                {
                    answer.ClarificationComments = null;
                    answer.ClarificationBy = null;
                    answer.ClarificationDate = null;
                }
            }

            bool updatedSuccessfully;
            if (isNew)
            {
                updatedSuccessfully = await _gatewayRepository.InsertGatewayPageAnswer(answer, request.UserId, request.UserName);
            }
            else
            {
                updatedSuccessfully = await _gatewayRepository.UpdateGatewayPageAnswer(answer, request.UserId, request.UserName);
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
           
            _auditService.Save();
            await _unitOfWork.Commit();

            return updatedSuccessfully;
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
