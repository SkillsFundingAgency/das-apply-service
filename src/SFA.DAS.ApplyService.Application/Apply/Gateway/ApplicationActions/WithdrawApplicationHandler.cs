using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationHandler : IRequestHandler<WithdrawApplicationRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<WithdrawApplicationHandler> _logger;
        private readonly IApplicationUpdatedEmailService _applicationUpdatedEmailService;

        public WithdrawApplicationHandler(IApplyRepository applyRepository, ILogger<WithdrawApplicationHandler> logger, IApplicationUpdatedEmailService applicationUpdatedEmailService)
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _applicationUpdatedEmailService = applicationUpdatedEmailService;
        }

        public async Task<bool> Handle(WithdrawApplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Performing Withdraw Application action for ApplicationId: {request.ApplicationId}");

            var result = await _applyRepository.WithdrawApplication(request.ApplicationId, request.Comments, request.UserId, request.UserName);

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

            return result;
        }
    }
}
