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
            var reviewOutcomes = new List<ModeratorPageReviewOutcome>();

            var sequences = await _sequenceService.GetSequences(applicationId);

            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    if (sequence.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = await _sectorService.GetSectorsForModerator(applicationId, moderatorUserId);
                        reviewOutcomes.AddRange(GeneratorSectorsReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, sectors, moderatorUserId));
                    }
                    else
                    {
                        reviewOutcomes.AddRange(await GeneratorSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, moderatorUserId));
                    }
                }
            }

            await _mediator.Send(new CreateEmptyModeratorReviewRequest(applicationId, moderatorUserId, reviewOutcomes));
        }

        private async Task<List<ModeratorPageReviewOutcome>> GeneratorSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, string moderatorUserId)
        {
            var sectionReviewOutcomes = new List<ModeratorPageReviewOutcome>();

            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var pages = section.QnAData.Pages;

            foreach (var page in pages)
            {
                sectionReviewOutcomes.Add(new ModeratorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = sequenceNumber,
                    SectionNumber = sectionNumber,
                    PageId = page.PageId,
                    UserId = moderatorUserId,
                    Status = null,
                    Comment = null,
                    ExternalComment = null
                });
            }

            return sectionReviewOutcomes;
        }

        private List<ModeratorPageReviewOutcome> GeneratorSectorsReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, List<AssessorSector> selectedSectors, string moderatorUserId)
        {
            var sectorReviewOutcomes = new List<ModeratorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                sectorReviewOutcomes.Add(new ModeratorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = sequenceNumber,
                    SectionNumber = sectionNumber,
                    PageId = sector.PageId,
                    UserId = moderatorUserId,
                    Status = null,
                    Comment = null,
                    ExternalComment = null
                });
            }

            return sectorReviewOutcomes;
        }
    }
}
