using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IUserService
    {
        Task<string> GetClaim(string claimName);
        Task<bool> ValidateUser(string user);
        Task<bool> AssociateOrgFromClaimWithUser();
        Task<Guid> GetUserId();
        Task<Guid> GetSignInId();
    }
}