using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorReviewCreationService : IAssessorReviewCreationService
    {
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IMediator _mediator;

        public AssessorReviewCreationService(IAssessorSequenceService sequenceService, IAssessorSectorService sectorService, IInternalQnaApiClient qnaApiClient, IMediator mediator)
        {
            _sequenceService = sequenceService;
            _sectorService = sectorService;
            _qnaApiClient = qnaApiClient;
            _mediator = mediator;
        }

        public async Task CreateEmptyReview(Guid applicationId, string assessorUserId, int assessorNumber)
        {
            var reviewOutcomes = new List<AssessorPageReviewOutcome>();

            var sequences = await _sequenceService.GetSequences(applicationId);

            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    if (sequence.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = await _sectorService.GetSectorsForAssessor(applicationId, assessorUserId);
                        reviewOutcomes.AddRange(GenerateSectorsReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, sectors, assessorUserId, assessorNumber));
                    }
                    else
                    {
                        reviewOutcomes.AddRange(await GenerateSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, assessorUserId, assessorNumber));
                    }
                }
            }

            await _mediator.Send(new CreateEmptyAssessorReviewRequest(applicationId, assessorUserId, reviewOutcomes));
        }

        private async Task<List<AssessorPageReviewOutcome>> GenerateSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, string assessorUserId, int assessorNumber)
        {
            var sectionReviewOutcomes = new List<AssessorPageReviewOutcome>();

            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var pages = section.QnAData.Pages;

            foreach (var page in pages)
            {
                sectionReviewOutcomes.Add(new AssessorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = sequenceNumber,
                    SectionNumber = sectionNumber,
                    PageId = page.PageId,
                    UserId = assessorUserId,
                    AssessorNumber = assessorNumber,
                    Status = null,
                    Comment = null
                });
            }

            return sectionReviewOutcomes;
        }

        private List<AssessorPageReviewOutcome> GenerateSectorsReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, List<AssessorSector> selectedSectors, string assessorUserId, int assessorNumber)
        {
            var sectorReviewOutcomes = new List<AssessorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                sectorReviewOutcomes.Add(new AssessorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = sequenceNumber,
                    SectionNumber = sectionNumber,
                    PageId = sector.PageId,
                    UserId = assessorUserId,
                    AssessorNumber = assessorNumber,
                    Status = null,
                    Comment = null
                });
            }

            return sectorReviewOutcomes;
        }
    }
}
