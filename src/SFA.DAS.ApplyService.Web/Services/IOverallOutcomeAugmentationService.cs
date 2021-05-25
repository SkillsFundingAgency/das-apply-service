using System.Threading.Tasks;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IOverallOutcomeAugmentationService
    {
        Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model,
            string userId);
    }
}