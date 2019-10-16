using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers
{
    public class SetPageAnswersResponse
    {
        public bool ValidationPassed { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        public string NextAction { get; set; }
        public string NextActionId { get; set; }

        public SetPageAnswersResponse()
        { }


        public SetPageAnswersResponse(string nextAction, string nextActionId)
        {
            ValidationPassed = true;
            NextAction = nextAction;
            NextActionId = nextActionId;
        }

        public SetPageAnswersResponse(List<KeyValuePair<string, string>> validationErrors)
        {
            ValidationErrors = validationErrors;
            ValidationPassed = false;
        }
    }
}