using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IAppealsApiClient
    {
        Task<GetAppealResponse> GetAppeal(Guid applicationId);
        Task<bool> MakeAppeal(Guid applicationId, string howFailedOnPolicyOrProcesses, string howFailedOnEvidenceSubmitted, string signinId, string userName);
    }
}