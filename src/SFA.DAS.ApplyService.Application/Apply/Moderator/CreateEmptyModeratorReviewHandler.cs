using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class CreateEmptyModeratorReviewHandler : IRequestHandler<CreateEmptyModeratorReviewRequest>
    {
        private readonly ILogger<CreateEmptyModeratorReviewHandler> _logger;
        private readonly IApplyRepository _applyRepository;
        private readonly IModeratorRepository _moderatorRepository;


        public CreateEmptyModeratorReviewHandler(ILogger<CreateEmptyModeratorReviewHandler> logger, IApplyRepository applyRepository, IModeratorRepository repository)
        {
            _logger = logger;
            _applyRepository = applyRepository;
            _moderatorRepository = repository;
        }

        public async Task<Unit> Handle(CreateEmptyModeratorReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateEmptyModeratorReview for ApplicationId '{request.ApplicationId}'");
            await _moderatorRepository.CreateEmptyModeratorReview(request.ApplicationId, request.ModeratorUserId, request.ModeratorUserName, request.PageReviewOutcomes);

            // APR-1945 - Show details of last person to do Moderation (in this case it's empty as no-one has started it yet)
            var applyData = await _applyRepository.GetApplyData(request.ApplicationId);
            applyData.ModeratorReviewDetails = new ModeratorReviewDetails();

            await _moderatorRepository.UpdateModerationStatus(request.ApplicationId, applyData, ModerationStatus.New, request.ModeratorUserId);

            return Unit.Value;
        }
    }
}
