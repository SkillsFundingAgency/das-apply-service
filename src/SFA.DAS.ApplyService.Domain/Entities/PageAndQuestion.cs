namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class PageAndQuestion
    {
        public PageAndQuestion(string pageId, string questionId)
        {
            PageId = pageId;
            QuestionId = questionId;
        }

        public string PageId { get; }
        public string QuestionId { get; }
    }
}
