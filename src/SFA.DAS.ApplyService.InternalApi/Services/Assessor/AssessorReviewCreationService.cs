using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorReviewCreationService : IAssessorReviewCreationService
    {
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IMediator _mediator;

        public AssessorReviewCreationService(IAssessorSequenceService sequenceService, IAssessorSectorService sectorService, IMediator mediator)
        {
            _sequenceService = sequenceService;
            _sectorService = sectorService;
            _mediator = mediator;
        }

        public async Task CreateEmptyReview(Guid applicationId, string assessorUserId, string assessorUserName, int assessorNumber)
        {
            var reviewOutcomes = new List<AssessorPageReviewOutcome>();

            var sequences = await _sequenceService.GetSequences(applicationId);

            foreach (var sequence in sequences)
            {
                var sectionsToBeReviewed = sequence.Sections.Where(sec => sec.Status != AssessorReviewStatus.NotRequired);
                foreach (var section in sectionsToBeReviewed)
                {
                    if (section.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = _sectorService.GetSectorsForEmptyReview(section);
                        reviewOutcomes.AddRange(GenerateSectorsReviewOutcomes(applicationId, section, sectors, assessorUserId, assessorNumber));
                    }
                    else
                    {
                        var sectionReviewOutcomes = GenerateSectionReviewOutcomes(applicationId, section, assessorUserId, assessorNumber);
                        reviewOutcomes.AddRange(sectionReviewOutcomes);
                    }
                }
            }

            await _mediator.Send(new CreateEmptyAssessorReviewRequest(applicationId, assessorUserId, assessorUserName, reviewOutcomes));
        }

        private List<AssessorPageReviewOutcome> GenerateSectionReviewOutcomes(Guid applicationId, AssessorSection section, string assessorUserId, int assessorNumber)
        {
            var sectionReviewOutcomes = new List<AssessorPageReviewOutcome>();

            if (section.Pages != null)
            {
                foreach (var page in section.Pages)
                {
                    sectionReviewOutcomes.Add(new AssessorPageReviewOutcome
                    {
                        ApplicationId = applicationId,
                        SequenceNumber = section.SequenceNumber,
                        SectionNumber = section.SectionNumber,
                        PageId = page.PageId,
                        UserId = assessorUserId,
                        AssessorNumber = assessorNumber,
                        Status = null,
                        Comment = null
                    });
                }
            }

            return sectionReviewOutcomes;
        }

        private List<AssessorPageReviewOutcome> GenerateSectorsReviewOutcomes(Guid applicationId, AssessorSection section, List<AssessorSector> selectedSectors, string assessorUserId, int assessorNumber)
        {
            var sectorReviewOutcomes = new List<AssessorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                sectorReviewOutcomes.Add(new AssessorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = section.SequenceNumber,
                    SectionNumber = section.SectionNumber,
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
