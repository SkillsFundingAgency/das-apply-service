using System;
using System.Linq;
using System.Security.Claims;

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
            string value = principal.FindFirstValue("Email");

            return value;
        }

        public static string GetSignInId(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("sub");

            return value;
        }

        public static bool SignedInFromApply(this ClaimsPrincipal principal)
        {
            return !principal.HasClaim(x => x.Type == "http://schemas.portal.com/ukprn");
        }
    }
}
