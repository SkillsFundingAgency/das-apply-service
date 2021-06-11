using Microsoft.AspNetCore.Authorization;
namespace SFA.DAS.ApplyService.InternalApi.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder AllowAnonymousUser(this AuthorizationPolicyBuilder builder)
        {
            builder.Requirements.Add(new NoneRequirement());
            return builder;
        }
    }
}
