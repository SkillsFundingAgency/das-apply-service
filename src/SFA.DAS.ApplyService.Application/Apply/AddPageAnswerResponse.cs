using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply
{
    // Derived from SFA.DAS.QnA.Api.Types, at some point we should be able to use the nuget
    public class AddPageAnswerResponse
    {
        public Page Page { get; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; }

        public AddPageAnswerResponse(List<KeyValuePair<string, string>> validationErrors)
        {
            ValidationErrors = validationErrors;
            Success = false;
        }

        public AddPageAnswerResponse(Page page)
        {
            Page = page;
            Success = true;
        }

        public bool Success { get; set; }
    }
}
