using System;
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
        public Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            return GetClaims(tokenValidatedContext?.Principal);
        }

        public Task<IEnumerable<Claim>> GetClaims(ClaimsPrincipal principal)
        {
            var email = principal?.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
            var claims = new List<Claim>
            {
                new ("Email",$"{email}"),
            };

            var sub = principal?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                      ?? principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? Guid.NewGuid().ToString();

            claims.Add(new Claim("UserId", sub));
            claims.Add(new Claim("sub", sub));

            return Task.FromResult<IEnumerable<Claim>>(claims);
        }
    }
}
