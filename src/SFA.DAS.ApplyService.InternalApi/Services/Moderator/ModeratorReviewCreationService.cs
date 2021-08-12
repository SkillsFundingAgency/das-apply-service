using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Moderator
{
    public class ModeratorReviewCreationService : IModeratorReviewCreationService
    {
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IMediator _mediator;

        public ModeratorReviewCreationService(IAssessorSequenceService sequenceService, IAssessorSectorService sectorService, IMediator mediator)
        {
            _sequenceService = sequenceService;
            _sectorService = sectorService;
            _mediator = mediator;
        }

        public async Task CreateEmptyReview(Guid applicationId, string moderatorUserId, string moderatorUserName)
        {
            var reviewOutcomes = new List<ModeratorPageReviewOutcome>();

            var sequences = await _sequenceService.GetSequences(applicationId);

            foreach (var sequence in sequences)
            {
                var sectionsToBeReviewed = sequence.Sections.Where(sec => sec.Status != AssessorReviewStatus.NotRequired);
                foreach (var section in sectionsToBeReviewed)
                {
                    if (section.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = _sectorService.GetSectorsForEmptyReview(section);
                        reviewOutcomes.AddRange(GenerateSectorsReviewOutcomes(applicationId, section, sectors, moderatorUserId));
                    }
                    else
                    {
                        var sectionReviewOutcomes = await GenerateSectionReviewOutcomes(applicationId, section, moderatorUserId);
                        reviewOutcomes.AddRange(sectionReviewOutcomes);
                    }
                }
            }

            await _mediator.Send(new CreateEmptyModeratorReviewRequest(applicationId, moderatorUserId, moderatorUserName, reviewOutcomes));
        }

        private async Task<List<ModeratorPageReviewOutcome>> GenerateSectionReviewOutcomes(Guid applicationId, AssessorSection section, string moderatorUserId)
        {
            var sectionReviewOutcomes = new List<ModeratorPageReviewOutcome>();

            if (section.Pages != null)
            {
                foreach (var page in section.Pages)
                {
                    sectionReviewOutcomes.Add(new ModeratorPageReviewOutcome
                    {
                        ApplicationId = applicationId,
                        SequenceNumber = section.SequenceNumber,
                        SectionNumber = section.SectionNumber,
                        PageId = page.PageId,
                        UserId = moderatorUserId,
                        Status = null,
                        Comment = null
                    });
                }

                if (section.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy)
                {
                    // Sadly we have to cater for existing applications that never had this page as part of an Blind Assessor check
                    if (await ShouldGenerateManagementHierarchFinancialPage(applicationId))
                    {
                        // Inject page to show Financial information to Moderator
                        sectionReviewOutcomes.Add(new ModeratorPageReviewOutcome
                        {
                            ApplicationId = applicationId,
                            SequenceNumber = section.SequenceNumber,
                            SectionNumber = section.SectionNumber,
                            PageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial,
                            UserId = moderatorUserId,
                            Status = null,
                            Comment = null
                        });
                    }
                }
            }

            return sectionReviewOutcomes;
        }

        private async Task<bool> ShouldGenerateManagementHierarchFinancialPage(Guid applicationId)
        {
            var request = new GetBlindAssessmentOutcomeRequest(applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy, RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial);
            var blindAssessmentOutcome = await _mediator.Send(request);
            return blindAssessmentOutcome != null;
        }

        private List<ModeratorPageReviewOutcome> GenerateSectorsReviewOutcomes(Guid applicationId, AssessorSection section, List<AssessorSector> selectedSectors, string moderatorUserId)
        {
            var sectorReviewOutcomes = new List<ModeratorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                sectorReviewOutcomes.Add(new ModeratorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    SequenceNumber = section.SequenceNumber,
                    SectionNumber = section.SectionNumber,
                    PageId = sector.PageId,
                    UserId = moderatorUserId,
                    Status = null,
                    Comment = null
                });
            }

            return sectorReviewOutcomes;
        }
    }
}
