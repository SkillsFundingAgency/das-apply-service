using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using NotRequiredOverrideConfiguration = SFA.DAS.ApplyService.Web.Configuration.NotRequiredOverrideConfiguration;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class NotRequiredOverridesService : INotRequiredOverridesService
    {
        private readonly List<NotRequiredOverrideConfiguration> _configuration;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly ISessionService _sessionService;
        private const string NotRequiredConfigSessionKeyFormat = "NotRequiredConfiguration_{0}";
        
        public NotRequiredOverridesService(IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, 
                                           IApplicationApiClient applicationApiClient,
                                           IQnaApiClient qnaApiClient,
                                           ISessionService sessionService)
        {
            _configuration = notRequiredOverrides.Value;
            _applicationApiClient = applicationApiClient;
            _qnaApiClient = qnaApiClient;
            _sessionService = sessionService;
        }

         public void RefreshNotRequiredOverrides(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);
            var configuration = CalculateNotRequiredOverrides(applicationId);
            var applicationNotRequiredOverrides = new Application.Apply.Roatp.NotRequiredOverrideConfiguration
            {
                NotRequiredOverrides = Mapper.Map<List<NotRequiredOverride>>(configuration)
            };
            _applicationApiClient.UpdateNotRequiredOverrides(applicationId, applicationNotRequiredOverrides);
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

        private List<NotRequiredOverrideConfiguration> CalculateNotRequiredOverrides(Guid applicationId)
        {
            List<NotRequiredOverrideConfiguration> configuration = null;

            var applicationData = _qnaApiClient.GetApplicationData(applicationId).GetAwaiter().GetResult() as JObject;


            if (applicationData != null)
            {
                var applicationNotRequiredOverrides =
                    _applicationApiClient.GetNotRequiredOverrides(applicationId).Result;

                if (applicationNotRequiredOverrides == null)
                {
                    configuration = new List<NotRequiredOverrideConfiguration>(_configuration);
                }
                else
                {
                    var appDataNotRequiredOverrides = applicationNotRequiredOverrides.NotRequiredOverrides.ToList();
                    configuration = Mapper.Map<List<NotRequiredOverrideConfiguration>>(appDataNotRequiredOverrides);
                }

                foreach (var overrideConfig in configuration)
                {
                    foreach (var condition in overrideConfig.Conditions)
                    {
                        var applicationDataValue = applicationData[condition.ConditionalCheckField];
                        condition.Value = applicationDataValue != null
                            ? applicationDataValue.Value<string>()
                            : string.Empty;
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
