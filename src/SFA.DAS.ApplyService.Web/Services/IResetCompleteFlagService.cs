using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IResetCompleteFlagService
    {
         Task ResetPagesComplete(Guid applicationId, string pageId);
    }
}