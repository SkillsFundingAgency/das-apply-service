using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.ApplyService.Web.StartupExtensions
{
    public class CustomClaims : ICustomClaims
    {
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var userId = tokenValidatedContext?.Principal?.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            var email = tokenValidatedContext?.Principal?.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;

            return await Task.FromResult<IEnumerable<Claim>>(new List<Claim>
            {
                new ("UserId",$"{userId}"),
                new ("Email",$"{email}"),
                new ("sub",$"{userId}")
            });
        }
    }
}
