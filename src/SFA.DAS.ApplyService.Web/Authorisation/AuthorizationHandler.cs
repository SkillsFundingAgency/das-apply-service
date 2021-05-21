﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Authorization
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationApiClient _apiClient;
        private readonly IUserService _userService;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            IApplicationApiClient apiClient,
            IUserService userService,
            ILogger<AuthorizationHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiClient = apiClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            _logger.LogInformation("AccessApplicationRequirementHandler invoked");

            var pendingRequirements = context.PendingRequirements.ToList();
            if (!pendingRequirements.Exists(x => x is AccessApplicationRequirement || x is ApplicationStatusRequirement))
            {
                return;
            }

            var requestedApplicationId = GetRequestedApplicationId();
            Apply application = null;

            if (Guid.TryParse(requestedApplicationId, out var applicationId))
            {
                _logger.LogInformation($"Requested application {applicationId}");
                var signInId = await _userService.GetSignInId();
                application = await _apiClient.GetApplicationByUserId(applicationId, signInId);
            }
            else
            {
                _logger.LogInformation("Unable to determine ApplicationId parameter");
            }

            foreach (var requirement in pendingRequirements)
            {
                _logger.LogInformation($"Evaluating requirement: {requirement.GetType().Name}");

                if (requirement is AccessApplicationRequirement)
                {
                    if (application != null && application.ApplicationId == applicationId)
                    {
                        context.Succeed(requirement);
                        _logger.LogInformation($"Requirement {requirement.GetType().Name} met");
                    }
                    else
                    {
                        _logger.LogInformation($"Requirement {requirement.GetType().Name} not met");
                    }
                }

                if (requirement is ApplicationStatusRequirement statusRequirement)
                {
                    if (application != null && statusRequirement.Statuses.Contains(application.ApplicationStatus))
                    {
                        context.Succeed(requirement);
                        _logger.LogInformation($"Requirement {requirement.GetType().Name} met");
                    }
                    else
                    {
                        var requiredStatus = string.Join(",", statusRequirement.Statuses);
                        var actualStatus = application == null ? "null" : application?.ApplicationStatus;
                        _logger.LogInformation($"Requirement {requirement.GetType().Name} not met - application status required: one of [{requiredStatus}], actual {actualStatus}");
                    }
                }
            }
        }

        private string GetRequestedApplicationId()
        {
            var result = _httpContextAccessor.HttpContext.Request.Method == HttpMethod.Get.Method
                ? _httpContextAccessor.HttpContext.Request.Query["ApplicationId"].ToString()
                : _httpContextAccessor.HttpContext.Request.Form["ApplicationId"].ToString();

            if(string.IsNullOrWhiteSpace(result))
            {
                var path = _httpContextAccessor.HttpContext.Request.Path.Value;

                if (path.StartsWith("/Application/"))
                {
                    result = path.Split('/').Last();
                }
            }

            return result;
        }
    }
}