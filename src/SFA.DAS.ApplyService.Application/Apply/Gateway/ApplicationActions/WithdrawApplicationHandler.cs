using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationHandler : IRequestHandler<WithdrawApplicationCommand, bool>
    {
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<WithdrawApplicationHandler> _logger;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;

        public WithdrawApplicationHandler(IGatewayRepository gatewayRepository, ILogger<WithdrawApplicationHandler> logger, IOversightReviewRepository oversightReviewRepository, IAuditService auditService, IApplicationUpdatedEmailService applicationUpdatedEmailService)
        {
            _gatewayRepository = gatewayRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _auditService = auditService;
            _logger = logger;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
        }

        public async Task<bool> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Performing Withdraw Application action for ApplicationId: {request.ApplicationId}");

            var oversightReview = new OversightReview
            {
                ApplicationId = request.ApplicationId,
                Status = OversightReviewStatus.Withdrawn,
                InternalComments = request.Comments,
                UserId = request.UserId,
                UserName = request.UserName
            };

            var result = await _gatewayRepository.WithdrawApplication(request.ApplicationId, request.Comments, request.UserId, request.UserName);

            try
            {
                if (result)
                {
                    await _applicationUpdatedEmailService.SendEmail(request.ApplicationId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to send withdraw confirmation email for application: {request.ApplicationId}");
            }

            _auditService.StartTracking(UserAction.WithdrawApplication, request.UserId, request.UserName);
            _auditService.AuditInsert(oversightReview);
            _oversightReviewRepository.Add(oversightReview);
            _auditService.Save();

            return result;
        }
    }
}
