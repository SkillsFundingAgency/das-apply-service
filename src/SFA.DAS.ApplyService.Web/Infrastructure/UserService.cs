using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpContextAccessor contextAccessor, ILogger<UserService> logger)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        private Task<string> GetClaim(string claimName)
        {
            // try to get claim.
            var claim = _contextAccessor.HttpContext.User.FindFirst(claimName);

            if (claim == null)
            {
                throw new ArgumentException($"Claim {claimName} not found.");
            }

            return Task.FromResult(claim.Value);
        }

        public async Task<Guid> GetSignInId()
        {

            var signInId = Guid.Empty;
            try
            {
                var value = await GetClaim("sub");
                Guid.TryParse(value, out signInId);

            }
            catch (ArgumentException)
            {
                //Ignore should have beem already logged in getclaim
            }

            return signInId;
        }
    }
}