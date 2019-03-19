using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Session;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class UserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IApplicationApiClient _apiClient;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpContextAccessor contextAccessor, UsersApiClient usersApiClient, ISessionService sessionService, IApplicationApiClient apiClient, ILogger<UserService> logger)
        {
            _contextAccessor = contextAccessor;
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<string> GetClaim(string claimName)
        {
            // try to get claim.
            var claim = _contextAccessor.HttpContext.User.FindFirst(claimName);

            if (claim == null)
            {
                throw new ArgumentException($"Claim {claimName} not found.");
            }

            return claim.Value;
        }

        public async Task<bool> ValidateUser(string user)
        {
            try
            {
                if (!string.IsNullOrEmpty(await GetClaim("UserId")))
                {
                    //Check if user is associated with registered EPAO
                    //The result of this will be used to determine if common menu is shown
                    var ukPrn = await GetClaim(
                        "http://schemas.portal.com/ukprn");
                    if (!string.IsNullOrEmpty(ukPrn))
                        SetUserIsRegWithEpao(true);
                }
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("Claims where empty, can ignore");
                //Ignore
            }

            // User is already set so return valid otherwise continue
            if (!string.IsNullOrEmpty(user))
                return true;

            //Attempt to extract variable from claim incase called from Accessor
            try
            {
                if (!string.IsNullOrEmpty(await GetClaim("display_name")))
                {
                    var displayName = await GetClaim("display_name");
                    _sessionService.Set("LoggedInFromAssessor",true);
                    //May have empty strings
                    if (!string.IsNullOrEmpty(displayName))
                        _sessionService.Set("LoggedInUser", displayName);
                    else
                    {
                        _logger.LogInformation("Claims where empty and user was null so redirecting to postsignin");
                        return false;
                    }
                }
            }
            catch (ArgumentException e)
            {  
                //One of the Claims where null so redirect to postsignin
                _logger.LogInformation(e, "One of the claims did not exist");
                return false;
            }
            return true;
        }


        public async Task<bool> AssociateOrgFromClaimWithUser()
        {
            //Check if where comming from assessor as a new user associated with registered EPAO
            //if so try associating the registered org with existing user
            try
            {
                var ukPrn = await GetClaim(
                    "http://schemas.portal.com/ukprn");
                if (!string.IsNullOrEmpty(ukPrn))
                {
                    var signInId = await GetClaim("sub");
                    var contact = await _usersApiClient.GetUserBySignInId(signInId);
                    if (contact != null)
                    {
                        var orgFromUkprn = await _apiClient.GetOrganisationByUkprn(ukPrn);
                        await _usersApiClient.AssociateOrganisationWithUser(contact.Id, orgFromUkprn.Id);

                        return true;
                    }
                }
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("Faild to retrieve ukprn request from Assessor so just carry on.");
                //Ignore and just fall through
            }

            return false;
        }

        public void SetUserIsRegWithEpao(bool flag)
        {
            _sessionService.Set("UserRegWithEPAO", flag);
        }
    }
}