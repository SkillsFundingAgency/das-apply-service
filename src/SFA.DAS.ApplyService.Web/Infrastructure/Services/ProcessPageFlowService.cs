using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class ProcessPageService : IProcessPageService
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly List<TaskListConfiguration> _configuration;

        public ProcessPageService(IQnaApiClient qnaApiClient,
            IOptions<List<TaskListConfiguration>> configuration)
        {
            _qnaApiClient = qnaApiClient;
            _configuration = _configuration = configuration.Value;
        }

        public async Task<string> GetIntroductionPageIdForSection(Guid applicationId, int sequenceId, int providerTypeId)
        {
            var sequenceDescription = _configuration.FirstOrDefault(x => x.Id == sequenceId);

            if (sequenceDescription == null)
                return null;


            switch (providerTypeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                    return sequenceDescription.StartupPages.FirstOrDefault(x => x.ProviderType.ToLower().Contains("main"))
                        ?.PageId;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    return sequenceDescription.StartupPages.FirstOrDefault(x => x.ProviderType.ToLower().Contains("employer"))
                        ?.PageId;
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    return sequenceDescription.StartupPages.FirstOrDefault(x => x.ProviderType.ToLower().Contains("supporting"))
                        ?.PageId;
                default:
                    return null;
            }
        }


        public async Task<int> GetProviderTypeId(Guid applicationId)
        {
            var providerTypeId = 1;
            string pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionMain;

            var providerTypeAnswer =
                await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            if (providerTypeAnswer != null && !String.IsNullOrWhiteSpace(providerTypeAnswer.Value))
            {
                int.TryParse(providerTypeAnswer.Value, out providerTypeId);
            }

            return providerTypeId;
        }


        //    private readonly List<IntroductionPageProviderTypeLookup> _lookups = new List<IntroductionPageProviderTypeLookup>
        //    {
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 1,
        //            ProviderTypeId = 1,
        //            PageId = "10"
        //        },
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 1,
        //            ProviderTypeId = 2,
        //            PageId = "11"
        //        },
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 1,
        //            ProviderTypeId = 3,
        //            PageId = "12"
        //        },
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 4,
        //            ProviderTypeId = 1,
        //            PageId = "500"
        //        },
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 4,
        //            ProviderTypeId = 2,
        //            PageId = "510"
        //        },
        //        new IntroductionPageProviderTypeLookup
        //        {
        //            SequenceId = 4,
        //            ProviderTypeId = 3,
        //            PageId = "520"
        //        },
        //    };
        //}


        //public class IntroductionPageProviderTypeLookup
        //{
        //    public  int SequenceId { get; set; }
        //    public  int ProviderTypeId { get; set; }
        //    public  string PageId { get; set; }
        //}

    }

}
