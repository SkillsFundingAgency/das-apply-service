using SFA.DAS.ApplyService.Domain.Sectors;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorLookupService
    {
        string GetTitleForSequence(int sequenceId);
        string GetTitleForPage(string pageId);

        string GetLabelForQuestion(string questionId);
        string GetSectorNameForPage(string pageId);

        SectorQuestionIds GetSectorQuestionIdsForSectorPageId(string pageId);
    }
}