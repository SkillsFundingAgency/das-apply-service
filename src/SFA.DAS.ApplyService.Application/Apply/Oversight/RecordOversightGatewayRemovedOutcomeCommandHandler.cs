using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RecordOversightGatewayRemovedOutcomeCommandHandler : IRequestHandler<RecordOversightGatewayRemovedOutcomeCommand>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly ILogger<RecordOversightGatewayRemovedOutcomeCommandHandler> _logger;
        private readonly IAuditService _auditService;

        public RecordOversightGatewayRemovedOutcomeCommandHandler(IApplyRepository applyRepository,
            IOversightReviewRepository oversightReviewRepository,
            ILogger<RecordOversightGatewayRemovedOutcomeCommandHandler> logger,
            IAuditService auditService)
        {
            _applyRepository = applyRepository;
            _oversightReviewRepository = oversightReviewRepository;
            _logger = logger;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(RecordOversightGatewayRemovedOutcomeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording Oversight review status of Unsuccessful for gateway fail application Id {request.ApplicationId}");

            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application == null)
            {
                throw new InvalidOperationException($"Application {request.ApplicationId} not found");
            }

            var oversightReview = new OversightReview
            {
                ApplicationId = request.ApplicationId,
                Status = OversightReviewStatus.Removed,
                UserId = request.UserId,
                UserName = request.UserName,
            };

            _auditService.StartTracking(UserAction.RecordOversightGatewayRemovedOutcome, request.UserId, request.UserName);
            _auditService.AuditInsert(oversightReview);
            
            await _oversightReviewRepository.Add(oversightReview);
           
            await _auditService.Save();

            return Unit.Value;
        }
    }
}
