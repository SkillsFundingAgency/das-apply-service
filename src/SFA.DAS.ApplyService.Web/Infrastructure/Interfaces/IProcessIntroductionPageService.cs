using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IProcessIntroductionPageService
    {
        Task<string> GetIntroductionPageId(Guid applicationId, int sequenceId);
        Task<int> GetProviderTypeId(Guid applicationId);
    }
}
