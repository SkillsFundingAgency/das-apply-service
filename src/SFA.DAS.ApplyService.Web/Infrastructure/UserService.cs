using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Session;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class UserService : IUserService
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
                    var orgName = await GetClaim(
                        "http://schemas.portal.com/orgname");
                    if (!string.IsNullOrEmpty(orgName))
                    {
                        _sessionService.Set("OrganisationName", orgName);
                        _sessionService.Set("UserRegWithEPAO", true);
                    }
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

            //Attempt to extract variable from claim incase called from Assessor
            try
            {
                if (string.IsNullOrEmpty(await GetClaim("display_name")))
                {
                    _logger.LogInformation("Claims where empty and user was null so redirecting to postsignin");
                    return false;
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
                var orgName = await GetClaim(
                    "http://schemas.portal.com/orgname");
                if (!string.IsNullOrEmpty(orgName))
                {
                    var signInId = await GetClaim("sub");
                    var contact = await _usersApiClient.GetUserBySignInId(signInId);
                    if (contact != null)
                    {
                        var orgFromName = await _apiClient.GetOrganisationByName(orgName); 
                        if(orgFromName != null)
                            await _usersApiClient.AssociateOrganisationWithUser(contact.Id, orgFromName.Id);
                        else
                            return false;

                        return true;
                    }
                }
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("Faild to retrieve organisation from Assessor so just carry on for now.");
                //Ignore and just fall through
            }

            return false;
        }

        public async Task<Guid> GetUserId()
        {
            var value = await GetClaim("UserId");

            Guid.TryParse(value, out var userId);

            return userId;
        }

        public async Task<Guid> GetSignInId()
        {

            var signInId = Guid.Empty;
            try
            {
                var value = await GetClaim("sub");
                Guid.TryParse(value, out signInId);

            }
            catch (ArgumentException e)
            {
                //Ignore should have beem already logged in getclaim
            }

            return signInId;
        }
    }
}