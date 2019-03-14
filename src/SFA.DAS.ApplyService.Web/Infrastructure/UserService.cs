using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Session;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class UserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;

        public UserService(IHttpContextAccessor contextAccessor, UsersApiClient usersApiClient, ISessionService sessionService)
        {
            _contextAccessor = contextAccessor;
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
        }

        public async Task<string> GetClaim(string claimName)
        {
            // try to get claim.
            // if null, load extra claims then try again.
            var claim = _contextAccessor.HttpContext.User.FindFirst(claimName);
            if (claim == null)
            {
                await LoadClaims();
                claim = _contextAccessor.HttpContext.User.FindFirst(claimName);
            }

            if (claim == null)
            {
                throw new ArgumentException($"Claim {claimName} not found.");
            }

            return claim.Value;
        }

        private async Task LoadClaims()
        {
            var signInId = _contextAccessor.HttpContext.User.FindFirst("sub").Value;
            var user = await _usersApiClient.GetUserBySignInId(signInId);
            var identity = new ClaimsIdentity(new List<Claim>() {new Claim("UserId", user.Id.ToString())});
            _contextAccessor.HttpContext.User.AddIdentity(identity);
            await _contextAccessor.HttpContext.SignInAsync(_contextAccessor.HttpContext.User);

            _sessionService.Set("LoggedInUser", $"{user.GivenNames} {user.FamilyName}");
        }
    }
}