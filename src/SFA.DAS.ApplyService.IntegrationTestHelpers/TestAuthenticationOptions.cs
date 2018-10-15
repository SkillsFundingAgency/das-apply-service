using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SFA.DAS.ApplyService.IntegrationTestHelpers
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
        }, "test");
    }
}