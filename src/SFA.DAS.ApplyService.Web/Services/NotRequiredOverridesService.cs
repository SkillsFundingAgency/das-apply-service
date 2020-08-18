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

        //MFCMFC in master
         public void RefreshNotRequiredOverrides(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);
            var configuration = CalculateNotRequiredOverrides(applicationId);
            SaveConfigurationToCache(applicationId, configuration);
        }
        
        //// MFCMFC Version from 1152
        //public async Task<List<NotRequiredOverrideConfiguration>> GetNotRequiredOverrides(Guid applicationId)
        //{
        //    var sessionKey = string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
        //    var configuration = _sessionService.Get<List<NotRequiredOverrideConfiguration>>(sessionKey);
        //    if (configuration != null)
        //    {
        //        return configuration;
        //    }

        //    var applicationNotRequiredOverrides = await _applicationApiClient.GetNotRequiredOverrides(applicationId);
        //    if (applicationNotRequiredOverrides == null)
        //    {
        //        configuration = _configuration.Value;
        //        PopulateNotRequiredOverridesWithApplicationData(applicationId, configuration);

        //        applicationNotRequiredOverrides = new Application.Apply.Roatp.NotRequiredOverrideConfiguration
        //        {
        //            NotRequiredOverrides = Mapper.Map<List<NotRequiredOverride>>(configuration) 
        //        };
        //        await _applicationApiClient.UpdateNotRequiredOverrides(applicationId, applicationNotRequiredOverrides);
        //        var sessionConfig = Mapper.Map<List<NotRequiredOverrideConfiguration>>(applicationNotRequiredOverrides.NotRequiredOverrides);
        //        _sessionService.Set(sessionKey, sessionConfig);
        //    }
        //    return Mapper.Map<List<NotRequiredOverrideConfiguration>>(applicationNotRequiredOverrides.NotRequiredOverrides);
        //}


        //MFCMFC Version from master
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



        //MFCMFC from 1152
        //private void PopulateNotRequiredOverridesWithApplicationData(Guid applicationId, List<NotRequiredOverrideConfiguration> configuration)
        //{
        //    var applicationData = _qnaApiClient.GetApplicationData(applicationId).GetAwaiter().GetResult() as JObject;

        //    if (applicationData == null)
        //    {
        //        return;
        //    }

        //    foreach (var overrideConfig in configuration)
        //    {
        //        foreach (var condition in overrideConfig.Conditions)
        //        {
        //            var applicationDataValue = applicationData[condition.ConditionalCheckField];
        //            condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
        //        }
        //    }

        //    var sessionKey = string.Format(NotRequiredConfigSessionKeyFormat, applicationId);
        //    _sessionService.Set(sessionKey, _configuration);
        //}


        //MFCMFC from master
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
