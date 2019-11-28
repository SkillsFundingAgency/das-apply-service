using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class QuestionPropertyTokeniser : IQuestionPropertyTokeniser
    {
        private readonly IQnaApiClient _qnaApiClient;
        private const string StartTokenPattern = "{{";
        private const string EndTokenPattern = "}}";

        public QuestionPropertyTokeniser(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task<string> GetTokenisedValue(Guid applicationId, string tokenisedValue)
        {
            if (string.IsNullOrEmpty(tokenisedValue))
            {
                return string.Empty;
            }

            if (ContainsTokens(tokenisedValue))
            {
                var questionTagName = GetTokenIdentifier(tokenisedValue);

                var stringToReplace = $"{StartTokenPattern}{questionTagName}{EndTokenPattern}";

                var questionValue = await _qnaApiClient.GetAnswerByTag(applicationId, questionTagName);

                if (questionValue != null)
                {
                    var tokenReplacementValue = tokenisedValue.Replace(stringToReplace, questionValue.Value);
                    return tokenReplacementValue;
                }
            }

            return tokenisedValue;
        }

        private bool ContainsTokens(string value)
        {
            var startTokenPosition = value.IndexOf(StartTokenPattern);

            if (startTokenPosition < 0)
            {
                return false;
            }

            var searchStartPosition = startTokenPosition + StartTokenPattern.Length;

            var endTokenPosition = value.IndexOf(EndTokenPattern, searchStartPosition);

            return (endTokenPosition > 0);
        }

        private string GetTokenIdentifier(string value)
        {
            var startOfToken = value.Substring(value.IndexOf(StartTokenPattern) + 1);

            var tokenValue = startOfToken.Substring(1, startOfToken.IndexOf(EndTokenPattern) - 1);

            return tokenValue;
        }
    }
}
