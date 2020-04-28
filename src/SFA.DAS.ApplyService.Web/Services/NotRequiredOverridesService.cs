using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotRequiredOverrideConfiguration = SFA.DAS.ApplyService.Web.Configuration.NotRequiredOverrideConfiguration;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class NotRequiredOverridesService : INotRequiredOverridesService
    {
        private readonly IOptions<List<NotRequiredOverrideConfiguration>> _configuration;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly ISessionService _sessionService;
        private const string NotRequiredConfigSessionKeyFormat = "NotRequiredConfiguration_{0}";
        
        public NotRequiredOverridesService(IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, 
                                           IApplicationApiClient applicationApiClient,
                                           IQnaApiClient qnaApiClient,
                                           ISessionService sessionService)
        {
            _configuration = notRequiredOverrides;
            _applicationApiClient = applicationApiClient;
            _qnaApiClient = qnaApiClient;
            _sessionService = sessionService;
        }

        public async Task<List<NotRequiredOverrideConfiguration>> GetNotRequiredOverrides(Guid applicationId)
        {
            var sessionKey = string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
            var configuration = _sessionService.Get<List<NotRequiredOverrideConfiguration>>(sessionKey);
            if (configuration != null)
            {
                return configuration;
            }

            var applicationNotRequiredOverrides = await _applicationApiClient.GetNotRequiredOverrides(applicationId);
            if (applicationNotRequiredOverrides == null)
            {
                configuration = _configuration.Value;
                PopulateNotRequiredOverridesWithApplicationData(applicationId, configuration);

                applicationNotRequiredOverrides = new Application.Apply.Roatp.NotRequiredOverrideConfiguration
                {
                    NotRequiredOverrides = Mapper.Map<List<NotRequiredOverride>>(configuration) 
                };
                await _applicationApiClient.UpdateNotRequiredOverrides(applicationId, applicationNotRequiredOverrides);
                var sessionConfig = Mapper.Map<List<NotRequiredOverrideConfiguration>>(applicationNotRequiredOverrides.NotRequiredOverrides);
                _sessionService.Set(sessionKey, sessionConfig);
            }
            return Mapper.Map<List<NotRequiredOverrideConfiguration>>(applicationNotRequiredOverrides.NotRequiredOverrides);
        }

        private void PopulateNotRequiredOverridesWithApplicationData(Guid applicationId, List<NotRequiredOverrideConfiguration> configuration)
        {
            var applicationData =  _qnaApiClient.GetApplicationData(applicationId).GetAwaiter().GetResult() as JObject;

            if (applicationData == null)
            {
                return;
            }

            foreach (var overrideConfig in configuration)
            {
                foreach (var condition in overrideConfig.Conditions)
                {
                    var applicationDataValue = applicationData[condition.ConditionalCheckField];
                    condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                }
            }

            var sessionKey = string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
            _sessionService.Set(sessionKey, _configuration);
        }
    }
}
