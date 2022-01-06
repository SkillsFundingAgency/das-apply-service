using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IOverallOutcomeService
    {
        Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model, string userId); 
        Task<ApplicationSummaryViewModel> BuildApplicationSummaryViewModel(Guid applicationId, string emailAddress);
        Task<ApplicationSummaryWithModeratorDetailsViewModel> BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(Guid applicationId, string emailAddress);
        Task<OutcomeSectorDetailsViewModel> GetSectorDetailsViewModel(Guid applicationId, string pageId);

    }
}