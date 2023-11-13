using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.ApplyService.Web.StartupExtensions
{
    public class CustomClaims : ICustomClaims
    {
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var email = tokenValidatedContext?.Principal?.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;

            var contact = await GetContactDetails(tokenValidatedContext, email);
            var claims = new List<Claim>
            {
                new ("Email",$"{email}"),
            };
            if (contact is not null)
            {
                claims.Add(new Claim("UserId",$"{contact.Id}"));
                claims.Add(new Claim("sub",$"{contact.SigninId}"));
                claims.Add(new Claim(ClaimTypes.Name, $"{contact.GivenNames} {contact.FamilyName}"));
            }
            else
            {
                claims.Add(new Claim("sub",$"{Guid.NewGuid()}"));
            }
            
            return await Task.FromResult<IEnumerable<Claim>>(claims);
        }

        private static async Task<Contact> GetContactDetails(TokenValidatedContext tokenValidatedContext, string email)
        {
            var client = tokenValidatedContext.HttpContext.RequestServices.GetRequiredService<IUsersApiClient>();
            return await client.GetUserByEmail(email);
        }
    }
}
