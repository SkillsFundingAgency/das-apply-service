using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IModeratorRepository
    {
        Task CreateModeratorPageOutcomes(List<ModeratorPageReviewOutcome> assessorPageReviewOutcomes);
    }
}
