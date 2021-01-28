using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightGatewayFailOutcomeCommandHandler : IRequestHandler<RecordOversightGatewayFailOutcomeCommand>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly ILogger<RecordOversightGatewayFailOutcomeCommandHandler> _logger;
        private readonly IAuditService _auditService;

        public RecordOversightGatewayFailOutcomeCommandHandler(IApplyRepository applyRepository,
            IOversightReviewRepository oversightReviewRepository,
            ILogger<RecordOversightGatewayFailOutcomeCommandHandler> logger,
            IAuditService auditService)
        {
            _applyRepository = applyRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _logger = logger;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(RecordOversightGatewayFailOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of Unsuccessful for gateway fail application Id {request.ApplicationId}");

            var oversightReview = new OversightReview
            {
                ApplicationId = request.ApplicationId,
                Status = OversightReviewStatus.Unsuccessful,
                UserId = request.UserId,
                UserName = request.UserName,
            };

            _auditService.StartTracking(UserAction.RecordOversightGatewayFailOutcome, request.UserId, request.UserName);
            _auditService.AuditInsert(oversightReview);
            await _oversightReviewRepository.Add(oversightReview);
            await _auditService.Save();

            await _applyRepository.UpdateApplicationStatus(request.ApplicationId, ApplicationStatus.Rejected);

            return Unit.Value;
        }
    }
}
