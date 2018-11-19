using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IDfeSignInService
    {
        Task<InviteUserResponse> InviteUser(string email, string givenName, string familyName, Guid userId);
    }
}