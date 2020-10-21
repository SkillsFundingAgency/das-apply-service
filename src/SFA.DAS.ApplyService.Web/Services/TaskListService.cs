using System;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TaskListService : ITaskListService
    {
        public TaskList2ViewModel GetTaskList2ViewModel(Guid applicationId)
        {
            return new TaskList2ViewModel();
        }
    }

    public interface ITaskListService
    {
        TaskList2ViewModel GetTaskList2ViewModel(Guid applicationId);
    }
}
