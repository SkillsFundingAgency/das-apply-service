namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public class CustomValidationResult
    {
        public CustomValidationResult(string questionId)
        {
            IsValid = true;
            QuestionId = questionId;
            ErrorMessage = null;
        }

        public CustomValidationResult(string questionId, string errorMessage)
        {
            IsValid = false;
            QuestionId = questionId;
            ErrorMessage = errorMessage;
        }

        public bool IsValid { get; }
        public string QuestionId { get; }
        public string ErrorMessage { get; }
    }
}
