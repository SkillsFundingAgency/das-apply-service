using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationHandler : IRequestHandler<WithdrawApplicationRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<WithdrawApplicationHandler> _logger;

        public WithdrawApplicationHandler(IApplyRepository applyRepository, IOversightReviewRepository oversightReviewRepository, IAuditService auditService, ILogger<WithdrawApplicationHandler> logger)
        {
            _applyRepository = applyRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<bool> Handle(WithdrawApplicationRequest request, CancellationToken cancellationToken)
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

            var result = await _applyRepository.WithdrawApplication(request.ApplicationId, request.Comments, request.UserId, request.UserName);

            _auditService.StartTracking(UserAction.WithdrawApplication, request.UserId, request.UserName);
            _auditService.AuditInsert(oversightReview);
            await _oversightReviewRepository.Add(oversightReview);
            await _auditService.Save();

            return result;
        }
    }
}
