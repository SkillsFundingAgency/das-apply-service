namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IAssessorLookupService
    {
        string GetTitleForSequence(int sequenceId);
        string GetTitleForPage(string pageId);
        string GetLabelForQuestion(string questionId);
    }
}