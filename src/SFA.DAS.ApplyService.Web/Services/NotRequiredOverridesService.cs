using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class NotRequiredOverridesService : INotRequiredOverridesService
    {
        private readonly List<NotRequiredOverrideConfiguration> _configuration;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly ISessionService _sessionService;
        private const string NotRequiredConfigSessionKeyFormat = "NotRequiredConfiguration_{0}";

        // TODO: for story APR-1152, implement link to repo interface that:
        // 1. tries to fetch the config from the API repository
        // 2. if present, use as source of truth
        // 3. if not present, retrieve from appsettings.json, and store to repository via API
        
        public NotRequiredOverridesService(IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, 
                                           IQnaApiClient qnaApiClient,
                                           ISessionService sessionService)
        {
            _configuration = notRequiredOverrides.Value;
            _qnaApiClient = qnaApiClient;
            _sessionService = sessionService;
        }

        public void RefreshNotRequiredOverrides(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);
            var configuration = CalculateNotRequiredOverrides(applicationId);
            SaveConfigurationToCache(applicationId, configuration);
        }

        public async Task RefreshNotRequiredOverridesAsync(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);
            var configuration = await CalculateNotRequiredOverridesAsync(applicationId);
            SaveConfigurationToCache(applicationId, configuration);
        }

        public List<NotRequiredOverrideConfiguration> GetNotRequiredOverrides(Guid applicationId)
        {
            var configuration = RetrieveConfigurationFromCache(applicationId);

            if (configuration == null)
            {
                RefreshNotRequiredOverrides(applicationId);
                configuration = RetrieveConfigurationFromCache(applicationId);
            }

            return configuration;
        }

        public async Task<List<NotRequiredOverrideConfiguration>> GetNotRequiredOverridesAsync(Guid applicationId)
        {
            var configuration = RetrieveConfigurationFromCache(applicationId);

            if (configuration == null)
            {
                await RefreshNotRequiredOverridesAsync(applicationId);
                configuration = RetrieveConfigurationFromCache(applicationId);
            }

            return configuration;
        }


        private List<NotRequiredOverrideConfiguration> CalculateNotRequiredOverrides(Guid applicationId)
        {
            List<NotRequiredOverrideConfiguration> configuration = null;

            var applicationData = _qnaApiClient.GetApplicationData(applicationId).GetAwaiter().GetResult() as JObject;

            if (applicationData != null)
            {
                configuration = new List<NotRequiredOverrideConfiguration>(_configuration);

                foreach (var overrideConfig in configuration)
                {
                    foreach (var condition in overrideConfig.Conditions)
                    {
                        var applicationDataValue = applicationData[condition.ConditionalCheckField];
                        condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                    }
                }
            }

            return configuration;
        }

        private async Task<List<NotRequiredOverrideConfiguration>> CalculateNotRequiredOverridesAsync(Guid applicationId)
        {
            List<NotRequiredOverrideConfiguration> configuration = null;

            var applicationData = (await _qnaApiClient.GetApplicationData(applicationId)) as JObject;

            if (applicationData != null)
            {
                configuration = new List<NotRequiredOverrideConfiguration>(_configuration);

                foreach (var overrideConfig in configuration)
                {
                    foreach (var condition in overrideConfig.Conditions)
                    {
                        var applicationDataValue = applicationData[condition.ConditionalCheckField];
                        condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                    }
                }
            }

            return configuration;
        }

        #region cache function
        private static string GetSessionKey(Guid applicationId)
        {
            return string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
        }

        private List<NotRequiredOverrideConfiguration> RetrieveConfigurationFromCache(Guid applicationId)
        {
            var sessionKey = GetSessionKey(applicationId);
            return _sessionService.Get<List<NotRequiredOverrideConfiguration>>(sessionKey);
        }

        private void SaveConfigurationToCache(Guid applicationId, List<NotRequiredOverrideConfiguration> configuration)
        {
            var sessionKey = GetSessionKey(applicationId);
            _sessionService.Set(sessionKey, configuration);
        }

        private void RemoveConfigurationFromCache(Guid applicationId)
        {
            var sessionKey = GetSessionKey(applicationId);
            _sessionService.Remove(sessionKey);
        }
        #endregion
    }
}
