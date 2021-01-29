using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorSectorService: IAssessorSectorService
    {
        private const int SectorsSequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private const int SectorsSectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;

        private readonly IMediator _mediator;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public AssessorSectorService(IMediator mediator, IInternalQnaApiClient qnaApiClient)
        {
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
        }

        public async Task<List<AssessorSector>> GetSectorsForAssessor(Guid applicationId, string userId)
        {
            var startingPages = await GetStartingPagesFromApplication(applicationId);

            var sectors = startingPages?.Select(page => new AssessorSector { Title = page.LinkTitle, PageId = page.PageId }).ToList();

            if (sectors != null)
            {
                var sectionStatusesRequest = new GetAssessorPageReviewOutcomesForSectionRequest(applicationId, SectorsSequenceNumber, SectorsSectionNumber, userId);
                var sectionStatuses = await _mediator.Send(sectionStatusesRequest);

                if (sectionStatuses != null)
                {
                    foreach (var sector in sectors)
                    {
                        foreach (var sectorStatus in sectionStatuses.Where(sectorStatus => sector.PageId == sectorStatus.PageId))
                        {
                            sector.Status = sectorStatus.Status;
                        }
                    }
                }
            }

            return sectors;
        }

        public async Task<List<AssessorSector>> GetSectorsForModerator(Guid applicationId, string userId)
        {
            var startingPages = await GetStartingPagesFromApplication(applicationId);

            var sectors = startingPages?.Select(page => new AssessorSector { Title = page.LinkTitle, PageId = page.PageId }).ToList();

            if (sectors != null)
            {
                var sectionStatusesRequest = new GetModeratorPageReviewOutcomesForSectionRequest(applicationId, SectorsSequenceNumber, SectorsSectionNumber, userId);
                var sectionStatuses = await _mediator.Send(sectionStatusesRequest);

                if (sectionStatuses != null)
                {
                    foreach (var sector in sectors)
                    {
                        foreach (var sectorStatus in sectionStatuses.Where(sectorStatus => sector.PageId == sectorStatus.PageId))
                        {
                            sector.Status = sectorStatus.Status;
                        }
                    }
                }
            }

            return sectors;
        }

        public async Task<List<AssessorSector>> GetSectorsForClarification(Guid applicationId, string userId)
        {
            var startingPages = await GetStartingPagesFromApplication(applicationId);

            var sectors = startingPages?.Select(page => new AssessorSector { Title = page.LinkTitle, PageId = page.PageId }).ToList();

            if (sectors != null)
            {
                var sectionStatusesRequest = new GetClarificationPageReviewOutcomesForSectionRequest(applicationId, SectorsSequenceNumber, SectorsSectionNumber, userId);
                var sectionStatuses = await _mediator.Send(sectionStatusesRequest);

                if (sectionStatuses != null)
                {
                    foreach (var sector in sectors)
                    {
                        foreach (var sectorStatus in sectionStatuses.Where(sectorStatus => sector.PageId == sectorStatus.PageId))
                        {
                            sector.Status = sectorStatus.Status;
                        }
                    }
                }
            }

            return sectors;
        }

        public List<AssessorSector> GetSectorsForEmptyReview(AssessorSection section)
        {
            var startingPages = GetStartingPagesFromAssessorSection(section);

            return startingPages?.Select(page => new AssessorSector { Title = page.LinkTitle, PageId = page.PageId }).ToList();
        }

        private async Task<List<Page>> GetStartingPagesFromApplication(Guid applicationId)
        {
            var qnaSection = await _qnaApiClient.GetSectionBySectionNo(applicationId, SectorsSequenceNumber, SectorsSectionNumber);

            return GetStartingPages(qnaSection?.QnAData?.Pages);
        }

        private List<Page> GetStartingPagesFromAssessorSection(AssessorSection section)
        {
            var startingPages = new List<Page>();

            if(section != null && section.SequenceNumber == SectorsSequenceNumber && section.SectionNumber == SectorsSectionNumber)
            {
                startingPages = GetStartingPages(section.Pages);
            }        

            return startingPages;
        }

        private List<Page> GetStartingPages(List<Page> qnaPages)
        {
            var startingPages = qnaPages?.Where(x =>
                x.DisplayType == SectionDisplayType.PagesWithSections
                && x.PageId != RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors
                && x.Active
                && x.Complete
                && !x.NotRequired);

            return startingPages?.ToList();
        }
    }
}