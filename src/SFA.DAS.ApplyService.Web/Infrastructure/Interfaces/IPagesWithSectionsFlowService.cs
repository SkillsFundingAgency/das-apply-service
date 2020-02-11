using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IPagesWithSectionsFlowService
    {
        ApplicationSection ProcessPagesInSectionsForStatusText(ApplicationSection selectedSection);
    }
}
