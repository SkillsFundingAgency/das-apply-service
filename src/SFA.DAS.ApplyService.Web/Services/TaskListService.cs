using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;

        public TaskListService(IApplicationApiClient apiClient, IQnaApiClient qnaApiClient)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
        }

        public async Task<TaskList2ViewModel> GetTaskList2ViewModel(Guid applicationId, Guid userId)
        {
            var result = new TaskList2ViewModel();

            var organisationDetails = await _apiClient.GetOrganisationByUserId(userId);
            var providerRoute = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            result.ApplicationSummaryViewModel = new ApplicationSummaryViewModel
            {
                UKPRN = organisationDetails.OrganisationUkprn?.ToString(),
                OrganisationName = organisationDetails.Name,
                TradingName = organisationDetails.OrganisationDetails?.TradingName,
                ApplicationRouteId = providerRoute.Value,
            };


            return result;
        }
    }

    public interface ITaskListService
    {
        Task<TaskList2ViewModel> GetTaskList2ViewModel(Guid applicationId, Guid userId);
    }
}
