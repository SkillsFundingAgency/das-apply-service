using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class NotRequiredOverridesService : INotRequiredOverridesService
    {
        private readonly List<Configuration.NotRequiredOverride> _configuration;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly ISessionService _sessionService;

        private const string NotRequiredConfigSessionKeyFormat = "NotRequiredConfiguration_{0}";
        
        public NotRequiredOverridesService(IOptions<List<Configuration.NotRequiredOverride>> notRequiredOverrides, 
                                           IApplicationApiClient applicationApiClient,
                                           IQnaApiClient qnaApiClient,
                                           ISessionService sessionService)
        {
            _configuration = notRequiredOverrides.Value;
            _applicationApiClient = applicationApiClient;
            _qnaApiClient = qnaApiClient;
            _sessionService = sessionService;
        }

        public async Task RefreshNotRequiredOverrides(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);

            var notRequiredOverrides = await CalculateNotRequiredOverrides(applicationId);

            if (await _applicationApiClient.UpdateNotRequiredOverrides(applicationId, notRequiredOverrides))
            {
                SaveConfigurationToCache(applicationId, notRequiredOverrides);
            }
        }

        public async Task<List<NotRequiredOverride>> GetNotRequiredOverrides(Guid applicationId)
        {
            var notRequiredOverrides = RetrieveConfigurationFromCache(applicationId);

            if (notRequiredOverrides is null)
            {
                await RefreshNotRequiredOverrides(applicationId);
                notRequiredOverrides = RetrieveConfigurationFromCache(applicationId);
            }

            return notRequiredOverrides;
        }

        private async Task<List<NotRequiredOverride>> CalculateNotRequiredOverrides(Guid applicationId)
        {
            List<NotRequiredOverride> notRequiredOverrides = null;

            var qnaApplicationData = await _qnaApiClient.GetApplicationData(applicationId) as JObject;

            if (qnaApplicationData != null)
            {
                var applicationNotRequiredOverrides = await _applicationApiClient.GetNotRequiredOverrides(applicationId);
                
                if (applicationNotRequiredOverrides is null)
                {
                    notRequiredOverrides = Mapper.Map<List<NotRequiredOverride>>(_configuration);
                }
                else
                {
                    notRequiredOverrides = new List<NotRequiredOverride>(applicationNotRequiredOverrides);
                }

                foreach (var notRequiredOverride in notRequiredOverrides)
                {
                    foreach (var condition in notRequiredOverride.Conditions)
                    {
                        var applicationDataValue = qnaApplicationData[condition.ConditionalCheckField];
                        condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                    }
                }
            }

            return notRequiredOverrides;
        }

        #region cache function
        private static string GetSessionKey(Guid applicationId)
        {
            return string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
        }

        private List<NotRequiredOverride> RetrieveConfigurationFromCache(Guid applicationId)
        {
            var sessionKey = GetSessionKey(applicationId);
            return _sessionService.Get<List<NotRequiredOverride>>(sessionKey);
        }

        private void SaveConfigurationToCache(Guid applicationId, List<NotRequiredOverride> configuration)
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
