using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IPageNavigationTrackingService
    {
        void AddPageToNavigationStack(string pageId);
        Task<string> GetBackNavigationPageId(Guid applicationId, int sequenceId, int sectionId, string pageId);
    }
}
