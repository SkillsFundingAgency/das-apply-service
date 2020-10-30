using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Orchestrators
{
    public interface ITaskListOrchestrator
    {
        Task<TaskListViewModel> GetTaskListViewModel(Guid applicationId, Guid userId);
    }
}