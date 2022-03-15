using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IResetQnaDetailsService
    {
         Task ResetPagesComplete(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
    }
}