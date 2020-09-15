using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Moderator
{
    public class ModeratorReviewCreationService : IModeratorReviewCreationService
    {
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IMediator _mediator;

        public ModeratorReviewCreationService(IAssessorSequenceService sequenceService, IAssessorSectorService sectorService, IInternalQnaApiClient qnaApiClient, IMediator mediator)
        {
            _sequenceService = sequenceService;
            _sectorService = sectorService;
            _qnaApiClient = qnaApiClient;
            _mediator = mediator;
        }

        public async Task CreateEmptyReview(Guid applicationId, string moderatorUserId)
        {
            var sequences = await _sequenceService.GetSequences(applicationId);
            var result = new List<ModeratorPageReviewOutcome>();

            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    if (sequence.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = await _sectorService.GetSectorsForModerator(applicationId, moderatorUserId);
                        result.AddRange(GetSectorsSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, sectors, moderatorUserId));
                    }
                    else
                    {
                        result.AddRange(await GetSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, moderatorUserId));
                    }
                }
            }

            await _mediator.Send(new CreateModeratorPageReviewOutcomesRequest
            {
                ModeratorPageReviewOutcomes = result
            });
        }

        private async Task<List<ModeratorPageReviewOutcome>> GetSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, string assessorUserId)
        {
            var result = new List<ModeratorPageReviewOutcome>();
            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var pages = section.QnAData.Pages;

            foreach (var page in pages)
            {
                result.Add(new ModeratorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    Comment = string.Empty,
                    ExternalComment = string.Empty,
                    PageId = page.PageId,
                    SectionNumber = sectionNumber,
                    SequenceNumber = sequenceNumber,
                    Status = string.Empty,
                    UserId = assessorUserId
                });
            }

            return result;
        }

        private List<ModeratorPageReviewOutcome> GetSectorsSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, List<AssessorSector> selectedSectors, string assessorUserId)
        {
            var result = new List<ModeratorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                result.Add(new ModeratorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    Comment = string.Empty,
                    ExternalComment = string.Empty,
                    PageId = sector.PageId,
                    SectionNumber = sectionNumber,
                    SequenceNumber = sequenceNumber,
                    Status = string.Empty,
                    UserId = assessorUserId
                });
            }

            return result;
        }
    }
}
