using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class NotRequiredOverridesService : INotRequiredOverridesService
    {
        private List<NotRequiredOverrideConfiguration> _configuration;
        private readonly IQnaApiClient _qnaApiClient;

        // TODO: implement repo interface that takes overrides from applydata if present

        public NotRequiredOverridesService(IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, IQnaApiClient qnaApiClient)
        {
            _configuration = notRequiredOverrides.Value;
            _qnaApiClient = qnaApiClient;
        }

        public List<NotRequiredOverrideConfiguration> GetNotRequiredOverrides(Guid applicationId)
        {
            PopulateNotRequiredOverridesWithApplicationData(applicationId);
            return _configuration;
        }

        private void PopulateNotRequiredOverridesWithApplicationData(Guid applicationId)
        {
            var applicationData =  _qnaApiClient.GetApplicationData(applicationId).GetAwaiter().GetResult() as JObject;

            if (applicationData == null)
            {
                return;
            }

            foreach (var overrideConfig in _configuration)
            {
                foreach (var condition in overrideConfig.Conditions)
                {
                    var applicationDataValue = applicationData[condition.ConditionalCheckField];
                    condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                }
            }

        }
    }
}
