using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class EmailAddressIsValidValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
           if (string.IsNullOrEmpty(answer?.Value)) return new List<KeyValuePair<string, string>>();

            return !IsValidEmail(answer.Value)
                ? new List<KeyValuePair<string, string>>
                    {new KeyValuePair<string, string>(answer.QuestionId, ValidationDefinition.ErrorMessage)}
                : new List<KeyValuePair<string, string>>();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var rx = new Regex(
                    @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return rx.IsMatch(email);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
