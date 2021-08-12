using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Mappers;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorPageService : IAssessorPageService
    {
        private readonly IMediator _mediator;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorSequenceService _assessorSequenceService;
        private readonly IAssessorLookupService _assessorLookupService;

        public AssessorPageService(IMediator mediator, IInternalQnaApiClient qnaApiClient, IAssessorSequenceService assessorSequenceService, IAssessorLookupService assessorLookupService)
        {
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
            _assessorSequenceService = assessorSequenceService;
            _assessorLookupService = assessorLookupService;
        }

        public async Task<AssessorPage> GetPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            AssessorPage page = null;

            if (_assessorSequenceService.IsValidSequenceNumber(sequenceNumber))
            {
                if (sequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining
                    && sectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy
                    && pageId == RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial)
                {
                    // Sadly we have to cater for existing applications that never had this page as part of an Blind Assessor check
                    if (await ShouldGetManagementHierarchFinancialPage(applicationId))
                    {
                        page = await GetManagementHierarchFinancialPage(applicationId);
                    }
                }
                else
                {
                    var qnaSection = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
                    var qnaPage = qnaSection?.QnAData.Pages.FirstOrDefault(p => p.PageId == pageId || string.IsNullOrEmpty(pageId));

                    if (qnaPage != null)
                    {
                        page = qnaPage.ToAssessorPage(_assessorLookupService, applicationId, sequenceNumber, sectionNumber);

                        var nextPageAction = await _qnaApiClient.SkipPageBySectionNo(page.ApplicationId, page.SequenceNumber, page.SectionNumber, page.PageId);

                        if (nextPageAction != null && NextAction.NextPage.Equals(nextPageAction.NextAction, StringComparison.InvariantCultureIgnoreCase))
                        {
                            page.NextPageId = nextPageAction.NextActionId;
                        }
                        else if (page.PageId == RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy)
                        {
                            // Move to injected page which shows Financial information to Assessor/Moderator
                            page.NextPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial;
                        }
                    }
                }
            }

            return page;
        }

        private async Task<AssessorPage> GetManagementHierarchFinancialPage(Guid applicationId)
        {
            AssessorPage page = null;

            var qnaSection = await _qnaApiClient.GetSectionBySectionNo(applicationId, RoatpWorkflowSequenceIds.FinancialEvidence, RoatpWorkflowSectionIds.FinancialEvidence.YourOrganisationsFinancialEvidence);
            var qnaPage = qnaSection?.QnAData.Pages.FirstOrDefault(p => p.PageId == RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_Other
                                                                    || p.PageId == RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_CompanyOrCharity
                                                                    || p.PageId == RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_SoleTraderOrPartnership);

            if (qnaPage != null)
            {
                // Modify the qnaPage so it shows only relevant questions to the assessor and has an appropriate page id
                qnaPage.Questions = qnaPage.Questions.Where(p => p.QuestionTag == RoatpWorkflowQuestionTags.Turnover || p.QuestionTag == RoatpWorkflowQuestionTags.AverageNumberofFTEEmployees).ToList();
                qnaPage.PageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial;

                page = qnaPage.ToAssessorPage(_assessorLookupService, applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);
            }

            return page;
        }

        private async Task<bool> ShouldGetManagementHierarchFinancialPage(Guid applicationId)
        {
            var request = new Application.Apply.Moderator.GetBlindAssessmentOutcomeRequest(applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy, RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial);
            var blindAssessmentOutcome = await _mediator.Send(request);
            return blindAssessmentOutcome != null;
        }
    }
}
