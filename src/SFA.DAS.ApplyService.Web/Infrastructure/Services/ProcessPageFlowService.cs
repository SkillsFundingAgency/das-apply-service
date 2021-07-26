using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class ProcessPageFlowService : IProcessPageFlowService
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly List<TaskListConfiguration> _configuration;

        public ProcessPageFlowService(IQnaApiClient qnaApiClient,
            IOptions<List<TaskListConfiguration>> configuration)
        {
            _qnaApiClient = qnaApiClient;
            _configuration = _configuration = configuration.Value;
        }

        public Task<string> GetIntroductionPageIdForSequence(int sequenceId,
            int providerTypeId)
        {
            var sequenceDescription = _configuration.FirstOrDefault(x => x.Id == sequenceId);

            return Task.FromResult(sequenceDescription?.StartupPages
                .FirstOrDefault(x => x.ProviderTypeId == providerTypeId)
                ?.PageId);
        }

        public async Task<int> GetApplicationProviderTypeId(Guid applicationId)
        {
            var providerTypeId = 0;
          
            var providerTypeAnswer =
                await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            if (providerTypeAnswer != null && !String.IsNullOrWhiteSpace(providerTypeAnswer.Value))
            {
                int.TryParse(providerTypeAnswer.Value, out providerTypeId);
            }

            return providerTypeId  == 0 ? 1 : providerTypeId;
        }
    }
}
