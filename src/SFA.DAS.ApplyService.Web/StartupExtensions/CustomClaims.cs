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

namespace SFA.DAS.ApplyService.Web.StartupExtensions;

public class CustomClaims : ICustomClaims
{
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
    {
        var email = tokenValidatedContext?.Principal?.Claims
            .FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))
            ?.Value ?? string.Empty;

        var contact = await GetContactDetails(tokenValidatedContext, email).ConfigureAwait(false);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };

        if (contact is not null)
        {
            claims.Add(new Claim("UserId", contact.Id.ToString()));
            claims.Add(new Claim("sub", contact.SigninId?.ToString() ?? Guid.NewGuid().ToString()));
            var displayName = $"{contact.GivenNames} {contact.FamilyName}".Trim();
            if (!string.IsNullOrEmpty(displayName))
            {
                claims.Add(new Claim(ClaimTypes.Name, displayName));
            }
        }
        else
        {
            var generatedId = Guid.NewGuid().ToString();
            claims.Add(new Claim("UserId", generatedId));
            claims.Add(new Claim("sub", generatedId));
        }

        return claims;
    }

    private static Task<Contact> GetContactDetails(TokenValidatedContext tokenValidatedContext, string email)
    {
        if (tokenValidatedContext is null) throw new ArgumentNullException(nameof(tokenValidatedContext));
        var client = tokenValidatedContext.HttpContext.RequestServices.GetRequiredService<IUsersApiClient>();
        return client.GetUserByEmail(email ?? string.Empty);
    }


    public Task<IEnumerable<Claim>> GetClaims(ClaimsPrincipal principal)
    {
        var email = principal?.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
        var name = principal?.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value
                   ?? principal?.Identity?.Name ?? string.Empty;
        var sub = principal?.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value
                  ?? Guid.NewGuid().ToString();

        var generatedId = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("UserId", generatedId),
            new Claim("sub", sub)
        };

        if (!string.IsNullOrEmpty(name))
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        return Task.FromResult<IEnumerable<Claim>>(claims);
    }

}
