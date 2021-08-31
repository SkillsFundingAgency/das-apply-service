using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IOverallOutcomeService
    {
        Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model,
            string userId); 
        Task<ApplicationSummaryViewModel> BuildApplicationSummaryViewModel(Apply application, string emailAddress);
        Task<ApplicationSummaryWithModeratorDetailsViewModel> BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(Apply application,
            string emailAddress);
        Task<OutcomeSectorDetailsViewModel> GetSectorDetailsViewModel(Guid applicationId, string pageId);

    }
}