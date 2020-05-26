using SFA.DAS.ApplyService.Domain.Sectors;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IAssessorLookupService
    {
        string GetTitleForSequence(int sequenceId);
        string GetTitleForPage(string pageId);

        SectorQuestionIds GetSectorQuestionIdsForSectorPageId(string pageId);
    }
}