using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IOverallOutcomeService
    {
        Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model,
            string userId);

        ApplicationSummaryViewModel BuildApplicationSummaryViewModel(Apply application, string emailAddress);

        Task<ApplicationSummaryWithModeratorDetailsViewModel> ApplicationUnsuccessful(Apply application,
            string emailAddress);
    }
}