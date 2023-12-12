using System;
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
            string value = principal.FindFirstValue("Email") ?? principal.FindFirstValue("name");

            return value;
        }

        public static Guid GetSignInId(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("sub");

            Guid.TryParse(value, out var signInId);

            return signInId;
        }
        public static string GetGovLoginId(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            return value;
        }

        public static string GetGivenName(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("given_name");

            return value;
        }

        public static string GetFamilyName(this ClaimsPrincipal principal)
        {
            string value = principal.FindFirstValue("family_name");

            return value;
        }
    }
}
