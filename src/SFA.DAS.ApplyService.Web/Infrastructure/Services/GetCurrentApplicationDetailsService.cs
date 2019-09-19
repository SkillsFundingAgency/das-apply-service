using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class GetCurrentApplicationDetailsService: IGetCurrentApplicationDetailsService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public GetCurrentApplicationDetailsService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task<int> GetProviderTypeId(Guid applicationId)
        {
            var providerTypeId = 1;
            string pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionMain;

            var providerTypeAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            if (providerTypeAnswer != null && !String.IsNullOrWhiteSpace(providerTypeAnswer.Value))
            {
                int.TryParse(providerTypeAnswer.Value, out providerTypeId);
            }

            return providerTypeId;
        }
    }
}
