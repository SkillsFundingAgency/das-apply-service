using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
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

            var allBlindAssessmentOutcomes = await GetAllBlindAssessmentOutcomes(applicationId);

            UpdateReviewOutcomesWithAutoPass(reviewOutcomes, allBlindAssessmentOutcomes);

            await _mediator.Send(new CreateEmptyModeratorReviewRequest(applicationId, moderatorUserId, moderatorUserName, reviewOutcomes));
        }

        private void UpdateReviewOutcomesWithAutoPass(List<ModeratorPageReviewOutcome> reviewOutcomes, List<BlindAssessmentOutcome> allBlindAssessmentOutcomes)
        {
            foreach(var blindOutcome in allBlindAssessmentOutcomes)
            {
                if (blindOutcome.Assessor1ReviewStatus == ModerationStatus.Pass && string.IsNullOrWhiteSpace(blindOutcome.Assessor1ReviewComment) 
                    && blindOutcome.Assessor2ReviewStatus == ModerationStatus.Pass && string.IsNullOrWhiteSpace(blindOutcome.Assessor2ReviewComment))
                {
                    var reviewOutcome = reviewOutcomes.FirstOrDefault(r => r.SequenceNumber == blindOutcome.SequenceNumber && r.SectionNumber == blindOutcome.SectionNumber && r.PageId == blindOutcome.PageId);
                    reviewOutcome.ModeratorReviewStatus = ModerationStatus.Pass;
                }
            }
        }

        private Task<List<BlindAssessmentOutcome>> GetAllBlindAssessmentOutcomes(Guid applicationId)
        {
            // get all blind assessments records : await _mediator.Send(new GetAllBlindAssessmentOutcomesRequest(applicationId));
            // foreach (var record in all blind assessments records)
            // check for ba1 pass + ba2 pass and no comments
            // update the record
            // await _mediator.Send(new CreateEmptyModeratorReviewRequest(applicationId, moderatorUserId, moderatorUserName, reviewOutcomes));
            return _mediator.Send(new GetAllBlindAssessmentOutcomesRequest(applicationId));
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
