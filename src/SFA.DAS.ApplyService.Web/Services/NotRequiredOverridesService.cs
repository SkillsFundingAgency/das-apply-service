using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NotRequiredOverridesService> _logger;

        private const string NotRequiredConfigSessionKeyFormat = "NotRequiredConfiguration_{0}";
        
        public NotRequiredOverridesService(IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, 
                                           IApplicationApiClient applicationApiClient,
                                           IQnaApiClient qnaApiClient,
                                           ISessionService sessionService, ILogger<NotRequiredOverridesService> logger)
        {
            _configuration = notRequiredOverrides.Value;
            _applicationApiClient = applicationApiClient;
            _qnaApiClient = qnaApiClient;
            _sessionService = sessionService;
            _logger = logger;
        }

        public void RefreshNotRequiredOverrides(Guid applicationId)
        {
            RemoveConfigurationFromCache(applicationId);
            var configuration =  CalculateNotRequiredOverrides(applicationId);
            _logger.LogDebug($"Mapping NotRequiredOverrides for Application Id [{applicationId}]");
            var applicationNotRequiredOverrides = new Application.Apply.Roatp.NotRequiredOverrideConfiguration
            {
                NotRequiredOverrides = Mapper.Map<List<NotRequiredOverride>>(configuration)
            };

            _logger.LogDebug($"Successfully mapped NotRequiredOverrides for Application Id [{applicationId}], Number of overrides: {applicationNotRequiredOverrides?.NotRequiredOverrides.Count()}");

            var updatedOverrides= _applicationApiClient.UpdateNotRequiredOverrides(applicationId, applicationNotRequiredOverrides).GetAwaiter().GetResult();
            _logger.LogDebug($"Updated Overrides for Application Id [{applicationId}], Result: {updatedOverrides}");
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
                _logger.LogDebug("Attempting to get overrides from application api for {applicationId}");
                var applicationNotRequiredOverrides = _applicationApiClient.GetNotRequiredOverrides(applicationId).GetAwaiter().GetResult();
                
                if (applicationNotRequiredOverrides == null)
                {
                    _logger.LogDebug("overrides from application api for {applicationId} is NULL");
                    configuration = new List<NotRequiredOverrideConfiguration>(_configuration);
                }
                else
                {
                    _logger.LogDebug("overrides from application api for {applicationId} is not null");
                    var appDataNotRequiredOverrides = applicationNotRequiredOverrides.NotRequiredOverrides.ToList();
                    _logger.LogDebug($"overrides from application api for {{applicationId}}, Count: {applicationNotRequiredOverrides?.NotRequiredOverrides.Count()}");
                    configuration = Mapper.Map<List<NotRequiredOverrideConfiguration>>(appDataNotRequiredOverrides);
                    _logger.LogDebug($"overrides from application api for {{applicationId}} mapped to configuration, Count [{configuration?.Count}]");
                }

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
