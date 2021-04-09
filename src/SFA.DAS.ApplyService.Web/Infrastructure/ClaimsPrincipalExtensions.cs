using System;
using System.Linq;
using System.Security.Claims;
using SFA.DAS.ApplyService.Web.Extensions;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("UserId");

            Guid.TryParse(value, out var userId);

            return userId;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("Email") ?? principal.FindFirstValue("name");

            return value;
        }

        public static string GetSignInId(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue(IdentityConstants.Subject);

            return value;
        }
    }
}
