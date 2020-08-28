using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Mappers;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorPageService: IAssessorPageService
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;

        public AssessorPageService(IInternalQnaApiClient qnaApiClient, IAssessorLookupService assessorLookupService)
        {
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
        }

        public async Task<AssessorPage> GetPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            AssessorPage page = null;

            var qnaSection =
                await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var qnaPage =
                qnaSection?.QnAData.Pages.FirstOrDefault(p => p.PageId == pageId || string.IsNullOrEmpty(pageId));

            if (qnaPage != null)
            {
                page = qnaPage.ToAssessorPage(_assessorLookupService, applicationId, sequenceNumber, sectionNumber);

                var nextPageAction = await _qnaApiClient.SkipPageBySectionNo(page.ApplicationId,
                    page.SequenceNumber, page.SectionNumber, page.PageId);

                if (nextPageAction != null && "NextPage".Equals(nextPageAction.NextAction,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    page.NextPageId = nextPageAction.NextActionId;
                }
            }
            
            return page;
        }
    }
}
