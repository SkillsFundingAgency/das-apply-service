using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IApplicationSectionOrchestrator
    {
        Task<ApplicationSectionPageResponse> GetFirstPage(GetApplicationSectionFirstPageRequest request);

        Task<ApplicationSectionPageResponse> GetNextPage(GetApplicationSectionNextPageRequest request);
    }
}
