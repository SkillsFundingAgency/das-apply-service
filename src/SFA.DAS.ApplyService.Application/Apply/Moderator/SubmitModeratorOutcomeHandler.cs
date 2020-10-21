using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class SubmitModeratorOutcomeHandler : IRequestHandler<SubmitModeratorOutcomeRequest, bool>
    {
        private readonly IApplyRepository _repository;
        private readonly IModeratorRepository _moderatorRepository;
        private readonly ILogger<SubmitModeratorOutcomeHandler> _logger;

        public SubmitModeratorOutcomeHandler(IApplyRepository repository, IModeratorRepository moderatorRepository, ILogger<SubmitModeratorOutcomeHandler> logger)
        {
            _repository = repository;
            _moderatorRepository = moderatorRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(SubmitModeratorOutcomeRequest request, CancellationToken cancellationToken)
        {

            var applyData = await _repository.GetApplyData(request.ApplicationId);

            var moderatorReviewDetails = applyData.ModeratorReviewDetails ?? new ModeratorReviewDetails();

            moderatorReviewDetails.ModeratorComments = request.Comment;
            moderatorReviewDetails.ModeratorName = request.UserName;
            moderatorReviewDetails.ModeratorUserId = request.UserId;
            moderatorReviewDetails.OutcomeDateTime = DateTime.UtcNow;
            if (request.Status == ModerationStatus.ClarificationSent)
                moderatorReviewDetails.ClarificationRequestedOn = DateTime.UtcNow;

            applyData.ModeratorReviewDetails = moderatorReviewDetails;

            _logger.LogInformation($"submitting moderator outcome for ApplicationId: {request.ApplicationId}");
            return await _moderatorRepository.SubmitModeratorOutcome(request.ApplicationId, applyData,
                request.UserId, request.Status);
        }
    }
}