using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IProcessPageService
    {
        Task<string> GetIntroductionPageIdForSection(Guid applicationId, int sequenceId, int providerTypeId);
        Task<int> GetProviderTypeId(Guid applicationId);
    }
}
