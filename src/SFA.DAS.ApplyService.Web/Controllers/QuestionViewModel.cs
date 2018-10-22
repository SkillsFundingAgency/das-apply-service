namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class QuestionViewModel
    {
        public string QuestionId { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Hint { get; set; }
        public string Value { get; set; }
        public dynamic Options { get; set; }
    }
}