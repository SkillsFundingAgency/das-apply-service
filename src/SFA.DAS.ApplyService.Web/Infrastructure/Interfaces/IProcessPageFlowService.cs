using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IProcessPageFlowService
    {
        Task<string> GetIntroductionPageIdForSequence(int sequenceId, int providerTypeId);
        Task<int> GetApplicationProviderTypeId(Guid applicationId);
    }
}
