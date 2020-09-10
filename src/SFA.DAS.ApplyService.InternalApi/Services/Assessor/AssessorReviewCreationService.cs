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
            var sequences = await _sequenceService.GetSequences(applicationId);
            var result = new List<AssessorPageReviewOutcome>();

            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    if (sequence.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining && section.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)
                    {
                        var sectors = await _sectorService.GetSectorsForAssessor(applicationId, assessorUserId);
                        result.AddRange(GetSectorsSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, sectors, assessorUserId, assessorNumber));
                    }
                    else
                    {
                        result.AddRange(await GetSectionReviewOutcomes(applicationId, sequence.SequenceNumber, section.SectionNumber, assessorUserId, assessorNumber));
                    }
                }
            }

            await _mediator.Send(new CreateAssessorPageReviewOutcomesRequest
            {
                AssessorPageReviewOutcomes = result
            });
        }

        private async Task<List<AssessorPageReviewOutcome>> GetSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, string assessorUserId, int assessorNumber)
        {
            var result = new List<AssessorPageReviewOutcome>();
            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var pages = section.QnAData.Pages;

            foreach (var page in pages)
            {
                result.Add(new AssessorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    AssessorNumber = assessorNumber,
                    Comment = string.Empty,
                    PageId = page.PageId,
                    SectionNumber = sectionNumber,
                    SequenceNumber = sequenceNumber,
                    Status = string.Empty,
                    UserId = assessorUserId
                });
            }

            return result;
        }

        private List<AssessorPageReviewOutcome> GetSectorsSectionReviewOutcomes(Guid applicationId, int sequenceNumber, int sectionNumber, List<AssessorSector> selectedSectors, string assessorUserId, int assessorNumber)
        {
            var result = new List<AssessorPageReviewOutcome>();

            foreach (var sector in selectedSectors)
            {
                result.Add(new AssessorPageReviewOutcome
                {
                    ApplicationId = applicationId,
                    AssessorNumber = assessorNumber,
                    Comment = string.Empty,
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
