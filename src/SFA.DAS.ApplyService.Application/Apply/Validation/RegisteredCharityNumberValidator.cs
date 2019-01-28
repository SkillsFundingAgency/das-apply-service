using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RegisteredCharityNumberValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            if (string.IsNullOrEmpty(answer?.Value)) return new List<KeyValuePair<string, string>>();

            return !IsValidRegisteredCharityNumber(answer.Value)
                ? new List<KeyValuePair<string, string>>
                    {new KeyValuePair<string, string>(answer.QuestionId, ValidationDefinition.ErrorMessage)}
                : new List<KeyValuePair<string, string>>();
        }

        private static bool IsValidRegisteredCharityNumber(string registeredCharityNumber)
        {
            try
            {
                var rx = new Regex(@"^[0-9]{7}$");

                if (registeredCharityNumber.Length==8)
                    registeredCharityNumber = registeredCharityNumber.Replace("-","");
                return rx.IsMatch(registeredCharityNumber);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
