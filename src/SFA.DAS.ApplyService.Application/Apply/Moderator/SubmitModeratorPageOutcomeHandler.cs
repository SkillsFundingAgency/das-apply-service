using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class SubmitModeratorPageOutcomeHandler : IRequestHandler<SubmitModeratorPageOutcomeRequest>
    {
        private readonly ILogger<SubmitModeratorPageOutcomeHandler> _logger;
        private readonly IApplyRepository _applyRepository;
        private readonly IModeratorRepository _moderatorRepository;

        public SubmitModeratorPageOutcomeHandler(ILogger<SubmitModeratorPageOutcomeHandler> logger, IApplyRepository applyRepository, IModeratorRepository moderatorRepository)
        {
            _logger = logger;
            _applyRepository = applyRepository;
            _moderatorRepository = moderatorRepository;
        }

        public async Task<Unit> Handle(SubmitModeratorPageOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"SubmitModeratorPageOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - Status '{request.Status}'");

            await _moderatorRepository.SubmitModeratorPageOutcome(request.ApplicationId, 
                                                        request.SequenceNumber, 
                                                        request.SectionNumber, 
                                                        request.PageId,  
                                                        request.UserId, 
                                                        request.UserName,
                                                        request.Status, 
                                                        request.Comment);

            // APR-1633 - Update Moderation Status from 'New' to 'In Progress'
            // APR-1945 - Update details of last person to do Moderation
            var applyData = await _applyRepository.GetApplyData(request.ApplicationId);
            var moderatorReviewDetails = applyData.ModeratorReviewDetails ?? new ModeratorReviewDetails();

            moderatorReviewDetails.ModeratorName = request.UserName;
            moderatorReviewDetails.ModeratorUserId = request.UserId;

            applyData.ModeratorReviewDetails = moderatorReviewDetails;
            await _moderatorRepository.UpdateModerationStatus(request.ApplicationId, applyData, ModerationStatus.InProgress, request.UserId);

            return Unit.Value;
        }
    }
}
