using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IUserService
    {
        Task<Guid> GetSignInId();
    }
}