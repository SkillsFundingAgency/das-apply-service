using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationHandler : IRequestHandler<WithdrawApplicationRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<WithdrawApplicationHandler> _logger;

        public WithdrawApplicationHandler(IApplyRepository applyRepository, ILogger<WithdrawApplicationHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(WithdrawApplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Performing Withdraw Application action for ApplicationId: {request.ApplicationId}");

            return await _applyRepository.WithdrawApplication(request.ApplicationId, request.Comments, request.UserId, request.UserName);
        }
    }
}
