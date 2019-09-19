using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class ProcessIntroductionPageService: IProcessIntroductionPageService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public ProcessIntroductionPageService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task<string> GetIntroductionPageId(Guid applicationId, int sequenceId)
        {
            var providerTypeId = await GetProviderTypeId(applicationId);
            return _lookups.FirstOrDefault(x => x.ProviderTypeId == providerTypeId && x.SequenceId == sequenceId)
                ?.PageId;
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
        

        private readonly List<IntroductionPageProviderTypeLookup> _lookups = new List<IntroductionPageProviderTypeLookup>
        {
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 1,
                ProviderTypeId = 1,
                PageId = "10"
            },
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 1,
                ProviderTypeId = 2,
                PageId = "11"
            },
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 1,
                ProviderTypeId = 3,
                PageId = "12"
            },
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 4,
                ProviderTypeId = 1,
                PageId = "500"
            },
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 4,
                ProviderTypeId = 2,
                PageId = "510"
            },
            new IntroductionPageProviderTypeLookup
            {
                SequenceId = 4,
                ProviderTypeId = 3,
                PageId = "520"
            },
        };
    }


    public class IntroductionPageProviderTypeLookup
    {
        public  int SequenceId { get; set; }
        public  int ProviderTypeId { get; set; }
        public  string PageId { get; set; }
    }


   
}
