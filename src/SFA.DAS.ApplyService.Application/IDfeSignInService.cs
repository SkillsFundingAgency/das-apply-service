using System;

namespace SFA.DAS.ApplyService.Application
{
    public interface IDfeSignInService
    {
        void InviteUser(string email, string givenName, string familyName, Guid userId);
    }
}