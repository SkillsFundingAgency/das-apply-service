namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string Label { get; set; }
        public string Hint { get; set; }
        public Input Input { get; set; }
        public int? Order { get; set; }
    }
}