using System;
using System.Collections.Generic;
using System.Globalization;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            var errorMessages = new List<KeyValuePair<string, string>>();

            var dateparts = answer.Value.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries);

            if (dateparts.Length != 3)
            {
                errorMessages.Add(new KeyValuePair<string, string>(question.QuestionId, ValidationDefinition.ErrorMessage));
                return errorMessages;
            }
            
            var day = dateparts[0];
            var month = dateparts[1];
            var year = dateparts[2];

            if (string.IsNullOrWhiteSpace(day) || string.IsNullOrWhiteSpace(month) || string.IsNullOrWhiteSpace(year))
            {
                errorMessages.Add(new KeyValuePair<string, string>(question.QuestionId, ValidationDefinition.ErrorMessage));
                return errorMessages;
            }

            var formatStrings = new string[] { "d/M/yy", "dd/M/yyyy", "d/MM/yy", "d/MM/yyyy", "dd/M/yy", "dd/M/yyyy", "dd/MM/yy", "dd/MM/yyyy" };
            if (!DateTime.TryParseExact($"{day}/{month}/{year}", formatStrings, null, DateTimeStyles.None, out _))
            {
                errorMessages.Add(new KeyValuePair<string, string>(question.QuestionId, ValidationDefinition.ErrorMessage));
            }
 
            return errorMessages;
        }
    }
}